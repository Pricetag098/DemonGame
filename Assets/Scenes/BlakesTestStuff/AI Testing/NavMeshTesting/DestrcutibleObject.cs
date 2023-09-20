using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DestrcutibleObject : Interactable
{
    public int Health;
    public int maxHealth;
    public int pointsToGain;
    public List<GameObject> symbolsList;
    public GameObject pentagramSymbol;

    public string interactMessage;

    private List<GameObject> activeSymbols = new List<GameObject>();

    private float rebuildTimer;
    [SerializeField] private float rebuildInterval;

    private bool canRebuild;

    private PlayerStats player;

    private void Awake()
    {
        activeSymbols.AddRange(symbolsList);

        player = FindObjectOfType<PlayerStats>();
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

        player.GainPoints(pointsToGain);

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

    public override void StartHover(Interactor interactor)
    {
        base.StartHover(interactor);
        interactor.display.DisplayMessage(true, interactMessage);
    }

    public override void EndHover(Interactor interactor)
    {
        base.EndHover(interactor);
        interactor.display.HideText();
    }
}
