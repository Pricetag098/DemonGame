using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBuy : ShopInteractable
{
    public bool open;

    [SerializeField] Optional<Area> area;

    protected override bool CanBuy(Interactor interactor)
    {
        return !open;
    }
    protected override void DoBuy(Interactor interactor)
    {
        open = true;
        //doAnimationStuff
        transform.parent.gameObject.SetActive(false);
    }

}
