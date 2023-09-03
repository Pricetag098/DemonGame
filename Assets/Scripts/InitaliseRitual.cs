using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitaliseRitual : Interactable
{
    SpawnerManager manager;
    [SerializeField] RitualSpawner spawner;

    private void Awake()
    {
        manager = FindObjectOfType<SpawnerManager>();    
    }

    public override void Interact(Interactor interactor)
    {
        if(spawner.ritualComplete == false && spawner.RitualActive == false)
        {
            manager.RunDefaultSpawning = false; // set default spawn off

            spawner.ritual = manager.Rituals[manager.RitualIndex]; // get the next ritual

            manager.DespawnAllActiveDemons();

            spawner.InitaliseRitual();

            manager.currentRitual = spawner;
        }
    }
}
