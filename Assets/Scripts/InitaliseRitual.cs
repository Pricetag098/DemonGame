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

            spawner.ritual = manager.Rituals[0]; // get the next ritual
            manager.Rituals.RemoveAt(0);

            manager.currentRitual = spawner; // set ritual

            manager.RitualSpawning = true; // active ritual
        }
    }
}
