using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuy : ShopInteractable
{
	[SerializeField] GameObject prefab;
	[SerializeField] string refillAmmoText;
	

	protected override bool CanBuy(Interactor interactor)
	{
		return true;
	}

	protected override void DoBuy(Interactor interactor)
	{
		Gun g;
		if (interactor.holster.HasGun(prefab.GetComponent<Gun>(),out g))
		{
			g.AddToStashPercent(1);
		}
		else
		{
            GameObject gun = Instantiate(prefab, interactor.holster.transform);
            interactor.holster.HeldGun = gun.GetComponent<Gun>();
        }
		
	}

	public override void StartHover(Interactor interactor)
	{
		base.StartHover(interactor);
		Gun g = prefab.GetComponent<Gun>();
        if (interactor.holster.HasGun(g))
        {
			interactor.display.DisplayMessage(true, refillAmmoText + " " + g.gunName + " ", "[Cost: " + GetCost(interactor).ToString()+ "]");
		}
        else
        {
			interactor.display.DisplayMessage(true, buyMessage + " " + g.gunName + " ", "[Cost: " + GetCost(interactor).ToString() + "]");

		}
		//interactor.display.DisplayMessage(true, interactor.holster.HasGun(g) ? (refillAmmoText + " " + g.gunName + ": ", GetCost(interactor).ToString()) : (buyMessage + " " + g.gunName + ": ", GetCost(interactor).ToString()));
		
	}

	protected override int GetCost(Interactor interactor)
	{
		return interactor.holster.HasGun(prefab.GetComponent<Gun>()) ? prefab.GetComponent<Gun>().refillCost : Cost;
	}
}
