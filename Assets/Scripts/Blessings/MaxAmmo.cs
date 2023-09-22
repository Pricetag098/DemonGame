using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxAmmo : BaseBlessing
{
    protected override void Activate(GameObject player)
    {
        Holster holster = player.GetComponentInChildren<Holster>();

        foreach (Transform obj in holster.transform)
        {
            if(obj.TryGetComponent(out Gun gun))
            {
                gun.RefillStash();
            }
        }
    }

}
