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
    // Start is called before the first frame update

    public void TakeDmg(float dmg)
    {
        health = Mathf.Clamp(health -dmg, 0, maxHealth);
        if(OnHit != null)
        OnHit();
        if(health <= 0)
		{
            Die();
		}
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
