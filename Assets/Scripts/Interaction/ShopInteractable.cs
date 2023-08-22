using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteractable : Interactable
{
    
	[SerializeField] protected int Cost;
	[SerializeField] protected string buyMessage = "To buy ";
	public override void Interact(Interactor interactor)
	{
		if (!CanBuy(interactor))
		{
            Debug.Log("cant buy");
            return;
        }
			
		if(interactor.playerStats.points >= GetCost(interactor) )
		{
			interactor.playerStats.SpendPoints(GetCost(interactor));
			DoBuy(interactor);
		}
	}

	protected virtual void DoBuy(Interactor interactor)
	{

	}

	protected virtual bool CanBuy(Interactor interactor)
	{
		return true;
	}

	public override void EndHover(Interactor interactor)
	{
		base.EndHover(interactor);
		interactor.display.HideText();
	}
	public override void StartHover(Interactor interactor)
	{
		base.StartHover(interactor);
		interactor.display.DisplayMessage(true,buyMessage + GetCost(interactor));
	}
	protected virtual string GetBuyMessage(Interactor interactor)
	{
		return " to buy " + GetCost(interactor);
	}
	protected virtual int GetCost(Interactor interactor)
	{
		return Cost;
	}
}
