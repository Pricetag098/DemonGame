using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillBlood : BaseBlessing
{
    protected override void Activate(GameObject player)
    {
        AbilityCaster abl = player.GetComponent<AbilityCaster>();
        abl.blood = abl.maxBlood;
    }

}
