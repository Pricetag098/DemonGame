using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUpgrade : ShopInteractable
{
    public string maxUpgradeText;
    protected override void DoBuy(Interactor interactor)
    {
        Gun heldGun = interactor.holster.HeldGun;
        GameObject newGun = heldGun.path.Value.path[heldGun.tier + 1];
        GameObject gunGo = Instantiate(newGun, interactor.holster.transform);
        Gun gun = gunGo.GetComponent<Gun>();
        interactor.holster.SetUpGun(gun);
        interactor.holster.ReplaceGun(gun);

    }
    public override void StartHover(Interactor interactor)
    {
        Gun heldGun = interactor.holster.HeldGun;
        base.StartHover(interactor);
        if(CanBuy(interactor))
        {
			interactor.display.DisplayMessage(true, buyMessage + " " + heldGun.gunName + ": " + GetCost(interactor));
        }
        else
        {
            interactor.display.DisplayMessage(false, maxUpgradeText);

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
