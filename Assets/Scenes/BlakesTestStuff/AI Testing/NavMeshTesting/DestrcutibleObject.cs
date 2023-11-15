using System.Collections;
using System.Collections.Generic;
using Unity.Physics;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DestrcutibleObject : Interactable
{
    public int Health;
    public int maxHealth;
    public int pointsToGain;
    public List<GameObject> symbolsList;
    public AudioSource audioSource;
    public List<AudioClip> wardingSounds;
    public GameObject pentagramSymbol;

    public string interactMessage;

    private List<GameObject> activeSymbols = new List<GameObject>();

    [SerializeField] private float rebuildInterval;

    private bool canRebuild;

    private PlayerStats player;
    private Timer timer;

    private Interactor InteractionHandler;

    private void Awake()
    {
        activeSymbols.AddRange(symbolsList);

        player = FindObjectOfType<PlayerStats>();

        timer = new Timer(rebuildInterval);
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
        if (Health > maxHealth) { Health = maxHealth; return; }

        activeSymbols[Health - 1].SetActive(true);
        audioSource.clip = wardingSounds[Random.Range(0, wardingSounds.Count)];
        audioSource.Play();
        if (Health == 1)
        {
            pentagramSymbol.SetActive(true);
            audioSource.clip = wardingSounds[Random.Range(0, wardingSounds.Count)];
            audioSource.Play();
        }

        player.GainPoints(pointsToGain);

        if(Health >= maxHealth) { InteractionHandler.display.HideText(); }
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
        canRebuild = true;
    }

    private void Update()
    {
        if(canRebuild == true)
        {
            if(timer.TimeGreaterThan)
            {
                RestoreHealth(1);
            }
        }
    }

    public override void StartHover(Interactor interactor)
    {
        base.StartHover(interactor);
        if(Health < maxHealth) interactor.display.DisplayMessage(true, interactMessage, null);

        InteractionHandler = interactor;
    }

    public override void EndHover(Interactor interactor)
    {
        base.EndHover(interactor);
        interactor.display.HideText();

        canRebuild = false;
        timer.SetTime(timer.TimeInterval);
    }
}
