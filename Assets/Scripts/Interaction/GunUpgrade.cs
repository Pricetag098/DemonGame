using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUpgrade : ShopInteractable
{
    protected override void DoBuy(Interactor interactor)
    {
        Gun heldGun = interactor.holster.HeldGun;
        GameObject newGun = heldGun.path.Value.path[heldGun.tier + 1];
        GameObject gun = Instantiate(newGun, interactor.holster.transform);
        interactor.holster.HeldGun = gun.GetComponent<Gun>();

    }
    public override void StartHover(Interactor interactor)
    {
        Gun heldGun = interactor.holster.HeldGun;
        base.StartHover(interactor);
        interactor.display.DisplayMessage(true, buyMessage + " " + heldGun.gunName + ": " + GetCost(interactor));

    }
    protected override bool CanBuy(Interactor interactor)
    {
        Gun heldGun = interactor.holster.HeldGun;
        if (heldGun.path.Enabled)
        {
            if (heldGun.path.Value.path.Count > heldGun.tier +1)
            {
                return true;
            }
        }
        return false;
    }

    
}
