using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitaliseRitual : RitualBase
{
    public override void OnAwake()
    {
        base.OnAwake();

    }

    public override void Interact(Interactor interactor)
    {
        if(spawner.ritualComplete == false && spawner.RitualActive == false)
        {
            manager.RunDefaultSpawning = false; // set default spawn off
            manager.DespawnAllActiveDemons(); // despawn all active demons

            //if(spawner.ritual is null)
            //{ 
            //spawner.ritual = manager.GetCurrentRitual();
            //spawner.IncrementRitual = true;
            //}

            spawner.InitaliseRitual(); // set ritual varibles

            manager.SetCurrentRitual(spawner);

            //manager.TpPlayerOnStart();
        }
    }

    public override void EndHover(Interactor interactor)
    {
        base.EndHover(interactor);
    }
    public override void StartHover(Interactor interactor)
    {
        base.StartHover(interactor);
    }

}
