using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestrcutibleObject : Interactable
{
    public int Health;
    public int maxHealth;
    public List<>

    public void TakeDamage(int Damage)
    {
        Health -= Damage;
        if(Health <= 0)
        {
            Health = 0;
        }
    }

    public void RestoreHealth(int amount)
    {
        Health += amount;
        if (Health > maxHealth) { Health = maxHealth; }
    }
    public void RestoreHealthToMax()
    {
        Health = maxHealth;
    }

    public override void Interact(Interactor interactor)
    {
        RestoreHealth(1); // add timer for this
    }
}
