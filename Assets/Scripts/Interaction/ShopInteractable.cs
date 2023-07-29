using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteractable : Interactable
{
    [SerializeField] protected PlayerStats playerStats;
	[SerializeField] int Cost;
	
	public override void Interact()
	{
		if (!CanBuy())
			return;
		if(playerStats.points >= Cost )
		{
			playerStats.points -= Cost;
			DoBuy();
			
		}
	}

	protected virtual void DoBuy()
	{

	}

	protected virtual bool CanBuy()
	{
		return true;
	}
}
