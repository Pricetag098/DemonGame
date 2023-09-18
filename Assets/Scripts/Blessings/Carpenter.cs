using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Carpenter : BaseBlessing
{
    protected override void Activate(GameObject player)
    {
        Spawners spawners = FindObjectOfType<Spawners>();

        foreach (DestrcutibleObject item in spawners.barriers)
        {
            item.RestoreHealthToMax();
        }
    }
}
