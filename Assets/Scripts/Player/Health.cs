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

    // Start is called before the first frame update
    public float regenPerSecond = 0;
    public float regenDelay = 0;
    float timeSinceLastHit;
    public Optional<VfxTargets> vfxTarget;

	private void Awake()
	{
        vfxTarget.Value = GetComponent<VfxTargets>();
        vfxTarget.Enabled = !(vfxTarget.Value is null);
	}
	public void TakeDmg(float dmg)
    {
        health = Mathf.Clamp(health -dmg, 0, maxHealth);
        if(OnHit != null)
        OnHit();
        if(health <= 0)
		{
            Die();
		}
        timeSinceLastHit = 0;
    }

    public void Respawn()
    {
        health = maxHealth;
        dead = false;
        if (OnRespawn != null)
            OnRespawn();
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
        //do die stuff
        //Debug.Log("dead",gameObject);
        if(OnDeath != null)
        OnDeath();
	}
}
