using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class DivineSmite : BaseBlessing
{
    public int pointsToGain = 500;

    protected override void Activate(GameObject player)
    {
        SpawnerManager man = FindObjectOfType<SpawnerManager>();

        man.KillAllActiveDemons();

        player.GetComponent<PlayerStats>().GainPoints(pointsToGain);
    }

}
