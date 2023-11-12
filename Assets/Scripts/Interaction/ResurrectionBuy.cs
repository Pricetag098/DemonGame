using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectionBuy : ShopInteractable
{
	PlayerDeath playerDeath;
	int buys = 0;
	int buyLimit = 3;
	public string cantBuyMessage;
	private void Awake()
	{
		playerDeath = FindObjectOfType<PlayerDeath>();
	}
	protected override bool CanBuy(Interactor interactor)
	{
		return playerDeath.respawnsLeft == 0 && buys < buyLimit;
	}

	protected override void DoBuy(Interactor interactor)
	{
		base.DoBuy(interactor);
		playerDeath.respawnsLeft++;
		buys++;
	}
}
