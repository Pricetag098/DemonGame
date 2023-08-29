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
