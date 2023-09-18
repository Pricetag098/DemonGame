using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DestrcutibleObject : Interactable
{
    public int Health;
    public int maxHealth;
    public List<GameObject> symbolsList;
    public GameObject pentagramSymbol;
    private List<GameObject> activeSymbols = new List<GameObject>();

    private float rebuildTimer;
    [SerializeField] private float rebuildInterval;

    private bool canRebuild;

    private void Awake()
    {
        activeSymbols.AddRange(symbolsList);
    }

    public void TakeDamage(int Damage)
    {
        int num = Health - 1;
        if(num >= 0)
        {
            activeSymbols[num].SetActive(false);
        }

        Health -= Damage;
        if(Health <= 0)
        {
            pentagramSymbol.SetActive(false);
            Health = 0;
        }
    }

    public void RestoreHealth(int amount)
    {
        Health += amount;
        activeSymbols[Health - 1].SetActive(true);
        if(Health == 1)
        {
            pentagramSymbol.SetActive(true);
        }

        if (Health > maxHealth) { Health = maxHealth; }
    }
    public void RestoreHealthToMax()
    {
        Health = maxHealth;
        pentagramSymbol.SetActive(true);
        for (int i = 0; i < activeSymbols.Count; i++)
        {
            activeSymbols[i].SetActive(true);
        }
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
