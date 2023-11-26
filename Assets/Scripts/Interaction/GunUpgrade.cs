using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUpgrade : ShopInteractable
{
    [SerializeField] GunUpPickup gunUpPickup;
    public string maxUpgradeText;
    protected override void DoBuy(Interactor interactor)
    {
        Gun heldGun = interactor.holster.HeldGun;
        GameObject newGun = heldGun.path.Value.path[heldGun.tier + 1];
        gunUpPickup.Infused(interactor, this, newGun, heldGun);
    }

    public override void StartHover(Interactor interactor)
    {
        Gun heldGun = interactor.holster.HeldGun;
        base.StartHover(interactor);
        if(CanBuy(interactor))
        {
			interactor.display.DisplayMessage(true, buyMessage + " " + heldGun.gunName + " ", "Cost: " + GetCost(interactor).ToString() + "]");
        }
        else
        {
            interactor.display.DisplayMessage(false, maxUpgradeText, null);

		}
        

    }

    protected override int GetCost(Interactor interactor)
    {
        return interactor.holster.HeldGun.upgradeCost;
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
