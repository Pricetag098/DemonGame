using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkBuy : ShopInteractable
{

	[SerializeField] public Perk perk;
	
	protected override void DoBuy(Interactor interactor)
	{
		interactor.perkManager.AddPerk(Instantiate(perk));
	}
	
	protected override bool CanBuy(Interactor interactor)
	{
		return !interactor.perkManager.HasPerk(perk);
	}

	
}
