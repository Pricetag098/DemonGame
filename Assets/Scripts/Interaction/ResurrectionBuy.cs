using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ResurrectionBuy : ShopInteractable
{
	PlayerDeath playerDeath;
	int buys = 0;
	int buyLimit = 3;
	public string usedUpMessage;
	public string alreadyOwnsMessage;
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
    public override void StartHover(Interactor interactor)
    {
        base.StartHover(interactor);
        
		if(buys >= buyLimit)
		{
            interactor.display.DisplayMessage(false, usedUpMessage);
        }
		else if(playerDeath.respawnsLeft >0)
		{
			interactor.display.DisplayMessage(false, alreadyOwnsMessage);
		}
		else
		{
			interactor.display.DisplayMessage(true, buyMessage);
		}
        
		

    }

    
}
