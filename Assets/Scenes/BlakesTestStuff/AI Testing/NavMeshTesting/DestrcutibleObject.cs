using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestrcutibleObject : Interactable
{
    public int Health;
    public int maxHealth;
    public List<>

    private float rebuildTimer;
    [SerializeField] private float rebuildInterval;

    private bool canRebuild;

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
        if (canRebuild == true)
        {
            RestoreHealth(1);

            canRebuild = false;
            rebuildTimer = 0;
        }
    }

    private void Update()
    {
        if(canRebuild == false)
        {
            canRebuild = rebuildTimer > rebuildInterval && Health < maxHealth;

            rebuildTimer += Time.deltaTime;   
        }
    }
}
