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

    // Start is called before the first frame update
    public float regenPerSecond = 0;
    public float regenDelay = 0;
    float timeSinceLastHit;
    public Optional<VfxTargets> vfxTarget;
    public HitType LastHitType;

	private void Awake()
	{
        vfxTarget.Value = GetComponent<VfxTargets>();
        vfxTarget.Enabled = !(vfxTarget.Value is null);
	}
	public bool TakeDmg(float dmg, HitType damageType)
    {
        if(dmg > damageLimit)
            dmg = damageLimit;

        health = Mathf.Clamp(health -dmg, 0, maxHealth);

        if(OnHit != null)
        {
            OnHit();
        }
        timeSinceLastHit = 0;
        if (health <= 0 && !dead)
		{
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
    }

	void Die()
	{
        if (dead)
            return;
        dead = true;

        bool grantPoints = false;

        if(gameObject.TryGetComponent<GrantPointsOnDeath>(out GrantPointsOnDeath points))
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
