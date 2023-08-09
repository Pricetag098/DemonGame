using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestrcutibleObject : MonoBehaviour
{
    public int Health;

    public delegate void TakeDamageEvent(float damage, float health);
    public TakeDamageEvent onTakeDamage;

    public void TakeDamage(int Damage)
    {
        Health -= Damage;
        if(Health <= 0)
        {
            Health = 0;
            onTakeDamage?.Invoke(Damage, Health);
            gameObject.SetActive(false);
        }
        else
        {
            onTakeDamage?.Invoke(Damage, Health);
        }
    }
}
