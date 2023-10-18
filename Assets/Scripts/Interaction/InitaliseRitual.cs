using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitaliseRitual : Interactable
{
    [SerializeField] protected string activateMessage = "To Activate ";

    private SpawnerManager manager;
    private RitualSpawner spawner;

    private void Awake()
    {
        manager = FindObjectOfType<SpawnerManager>();
        spawner = GetComponent<RitualSpawner>();
    }

    public override void Interact(Interactor interactor)
    {
        if(spawner.ritualComplete == false && spawner.RitualActive == false)
        {
            manager.RunDefaultSpawning = false; // set default spawn off

            if(spawner.ritual is null)
            { 
                spawner.ritual = manager.GetCurrentRitual();
                spawner.IncrementRitual = true;
            }

            manager.DespawnAllActiveDemons(); // despawn all active demons

            spawner.InitaliseRitual(); // set ritual varibles

            manager.SetCurrentRitual(spawner);

            manager.TpPlayerOnStart();
        }
    }

    public override void EndHover(Interactor interactor)
    {
        base.EndHover(interactor);
        interactor.display.HideText();
    }
    public override void StartHover(Interactor interactor)
    {
        base.StartHover(interactor);
        if(spawner.ritualComplete == false && spawner.RitualActive == false)
        {
            interactor.display.DisplayMessage(true, activateMessage);
        }
    }
}
