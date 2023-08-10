using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkBuy : ShopInteractable
{
	[SerializeField] PerkManager perkManager;
	[SerializeField] Perk perk;
	
	protected override void DoBuy(Interactor interactor)
	{
		perkManager.AddPerk(Instantiate(perk));
	}
	
	protected override bool CanBuy(Interactor interactor)
	{
		return !perkManager.HasPerk(perk);
	}

	
}
