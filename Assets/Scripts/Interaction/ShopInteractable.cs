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
			
		if(interactor.playerStats.points >= Cost )
		{
			interactor.playerStats.points -= Cost;
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
		interactor.display.DisplayMessage(true,buyMessage + Cost);
	}
	protected virtual string GetBuyMessage()
	{
		return " to buy " + Cost;
	}
}
