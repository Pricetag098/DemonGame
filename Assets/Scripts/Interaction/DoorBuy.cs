using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBuy : ShopInteractable
{
    public bool open;
    [SerializeField] Vector3 openOffset;
    [SerializeField] Area area;

    protected override bool CanBuy()
    {
        return !open;
    }
    protected override void DoBuy()
    {
        open = true;
        //doAnimationStuff
        transform.parent.position += openOffset;
        area.SpawnLocations();
    }
}
