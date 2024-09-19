using Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public bool dead;
    public delegate void Action();
    public Action OnDeath;
    public Action OnHit;
    public Action OnRespawn;
    public float damageLimit = float.PositiveInfinity;

    //used by abilities and melee attack to determine point gain
    public bool pointsOnHit = true;

    // Start is called before the first frame update
    public float regenPerSecond = 0;
    public float regenDelay = 0;
    public float invisTime = 0.1f;
    float timeSinceLastHit;
    public Optional<VfxTargets> vfxTarget;
    public HitType LastHitType;

    PlayerInputt playerInput;
    ShieldTracker shieldTracker;
    bool isPlayer;
    bool canUseShield;
    bool hasBeenHit;
    bool cantBeHit;

    float invisTimer;

	private void Awake()
	{
        vfxTarget.Value = GetComponent<VfxTargets>();
        vfxTarget.Enabled = !(vfxTarget.Value is null);

        if(TryGetComponent<PlayerInputt>(out PlayerInputt player))
        {
            playerInput = player;
            shieldTracker = GetComponent<ShieldTracker>();
            isPlayer = true;
        }
	}
	public bool TakeDmg(float dmg, HitType damageType)
    {
        if (isPlayer)
        {
            playerInput.GotHit();
            canUseShield = false;
            if (health > 1)
            {
                canUseShield = true;
            }
        }

        if (dmg > damageLimit)
            dmg = damageLimit;

        if (!cantBeHit)
        {
            health = Mathf.Clamp(health - dmg, 0, maxHealth);
        }

        if (isPlayer)
        {
            hasBeenHit = true;
            invisTimer = 0;
            cantBeHit = true;
        }

        if(OnHit != null)
        {
            OnHit();
        }
        timeSinceLastHit = 0;
        if (health <= 0 && !dead)
		{
            if (isPlayer)
            {
                if(canUseShield)
                {
                    if (shieldTracker.SpendShield())
                    {
                        health++;
                        return false;
                    }
                }
            }

            LastHitType = damageType;

            Die();
            return true;
        }
        return false;
        
    }

    public void Respawn()
    {
        health = maxHealth;
        dead = false;
        if (OnRespawn != null)
        {
            OnRespawn();
        } 
    }

	private void Update()
	{
        timeSinceLastHit += Time.deltaTime;
        if(timeSinceLastHit > regenDelay)
        health = Mathf.Clamp(health + regenPerSecond * Time.deltaTime, 0, maxHealth);

        if (hasBeenHit)
        {
            invisTimer += Time.deltaTime;
            if(invisTimer > invisTime)
            {
                hasBeenHit = false;
                cantBeHit = false;
            }
        }
    }

	void Die()
	{
        if (dead)
            return;
        dead = true;

        bool grantPoints = false;

        if(gameObject.TryGetComponent(out GrantPointsOnDeath points))
        {
            grantPoints = true;

            switch (LastHitType)
            {
                case HitType.Null:

                    break;
                case HitType.GUN:
                    OnDeath += points.AddPointsDeathGun;
                    break;
                case HitType.ABILITY:
                    OnDeath += points.AddPointsDeathAbility;
                    break;
            }
        }

        if (OnDeath != null)
        {
            OnDeath();
        }

        if(grantPoints == true)
        {
            switch (LastHitType)
            {
                case HitType.Null:

                    break;
                case HitType.GUN:
                    OnDeath -= points.AddPointsDeathGun;
                    break;
                case HitType.ABILITY:
                    OnDeath -= points.AddPointsDeathAbility;
                    break;
            }
        }
	}
}
