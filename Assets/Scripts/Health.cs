using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public delegate void Action();
    public Action OnDeath;
    public Action OnHit;
    // Start is called before the first frame update
    void Start()
    {
        //health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    void Die()
	{
        //do die stuff
        //Debug.Log("dead",gameObject);
        if(OnDeath != null)
        OnDeath();
	}
}
