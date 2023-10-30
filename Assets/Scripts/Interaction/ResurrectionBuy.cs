using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectionBuy : ShopInteractable
{
	PlayerDeath playerDeath;
	private void Awake()
	{
		playerDeath = FindObjectOfType<PlayerDeath>();
	}
	protected override bool CanBuy(Interactor interactor)
	{
		return playerDeath.respawnsLeft > 0;
	}

	protected override void DoBuy(Interactor interactor)
	{
		base.DoBuy(interactor);
		playerDeath.respawnsLeft++;
	}
}
