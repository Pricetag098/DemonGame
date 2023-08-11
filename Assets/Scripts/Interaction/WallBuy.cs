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
		interactor.display.DisplayMessage(true, interactor.holster.HasGun(prefab.GetComponent<Gun>()) ? refillAmmoText + Cost : buyMessage + Cost);
		
	}
}
