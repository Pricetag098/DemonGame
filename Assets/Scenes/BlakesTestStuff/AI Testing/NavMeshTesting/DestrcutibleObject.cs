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
    public List<WardFragment> partList;
    public AudioSource audioSource;
    public List<AudioClip> wardingSounds;
    public CrossDissolve crossSymbol;

    public string interactMessage;

    private List<WardFragment> activeParts = new List<WardFragment>();

    [SerializeField] private float rebuildInterval;
    [SerializeField] private float firstRebuildInterval;

    private bool canRebuild;

    private PlayerStats player;
    private Timer timer;

    private Interactor InteractionHandler;

    private BarrierTracker barrierTracker;

    private void Awake()
    {
        activeParts.AddRange(partList);

        player = FindObjectOfType<PlayerStats>();

        timer = new Timer(rebuildInterval);

        barrierTracker = player.GetComponent<BarrierTracker>();
    }

    public void TakeDamage(int Damage)
    {
        int num = Health - 1;
        if(num >= 1)
        {
            activeParts[num - 1].Off();
        }

        Health -= Damage;
        if(Health <= 0)
        {
            crossSymbol.Off();
            Health = 0;
        }
    }

    public void RestoreHealth(int amount)
    {
        Health += amount;
        if (Health > maxHealth) { Health = maxHealth; return; }
        if(Health >= 2)
        {
            activeParts[Health - 2].On();
        }
        audioSource.clip = wardingSounds[Random.Range(0, wardingSounds.Count)];
        audioSource.Play();
        if (Health == 1)
        {
            crossSymbol.On();
            audioSource.clip = wardingSounds[Random.Range(0, wardingSounds.Count)];
            audioSource.Play();
        }

        barrierTracker.Rebuilt();

        if(barrierTracker.CanRebuild())
        {
            player.GainPoints(pointsToGain);
        }

        if (Health >= maxHealth) { InteractionHandler.display.HideText(); }
    }
    public void RestoreHealthToMax()
    {
        Health = maxHealth;
        crossSymbol.On();
        for (int i = 0; i < activeParts.Count; i++)
        {
            activeParts[i].On();
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
                Debug.Log(timer.TimeInterval + " Rebuild Time");
            }
        }
    }

    public override void StartHover(Interactor interactor)
    {
        base.StartHover(interactor);
        if(Health < maxHealth) interactor.display.DisplayMessage(true, interactMessage, null);

        InteractionHandler = interactor;

        if (Health > 0)
        {
            timer.SetTimeInterval(rebuildInterval);
        }
        else
        {
            timer.SetTimeInterval(firstRebuildInterval);
        }
    }

    public override void EndHover(Interactor interactor)
    {
        base.EndHover(interactor);
        interactor.display.HideText();

        canRebuild = false;
        timer.SetTime(timer.TimeInterval);
    }
}
