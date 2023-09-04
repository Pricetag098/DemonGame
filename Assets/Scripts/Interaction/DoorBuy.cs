using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBuy : ShopInteractable
{
    public bool open;

    [SerializeField] Optional<Area> area1;
    [SerializeField] Optional<Area> area2;

    protected override bool CanBuy(Interactor interactor)
    {
        return !open;
    }
    protected override void DoBuy(Interactor interactor)
    {
        open = true;
        //doAnimationStuff
        transform.parent.gameObject.SetActive(false);

        //if(area1.Enabled) area1.Value.SpawnLocations();
        //if(area2.Enabled) area2.Value.SpawnLocations();
    }

}
