using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBuy : Interactable
{
	[SerializeField] List<int> costs;
	protected int Cost;
	[SerializeField] protected string buyMessage = "To buy ";
	[SerializeField] string hasMaxShieldText;
	[SerializeField] Optional<SoundPlayer> buySound;
	[SerializeField] Optional<SoundPlayer> tooExpensiveSound;

	private ShieldTracker shieldTracker;

	int shieldsBought;

	bool canBuy;

	public override void Interact(Interactor interactor)
	{
		if (!CanBuy(interactor))
		{
            Debug.Log("cant buy");
            return;
        }

		if (canBuy)
		{
			if (interactor.playerStats.points >= GetCost(interactor))
			{
				interactor.playerStats.SpendPoints(GetCost(interactor));
				if (buySound.Enabled)
					buySound.Value.Play();
				DoBuy(interactor);
			}
			else
			{
				if (tooExpensiveSound.Enabled)
					tooExpensiveSound.Value.Play();
				interactor.display.CantBuy();
				CantBuy(interactor);
			}
		}
    }

	protected virtual void CantBuy(Interactor interactor)
	{

	}

	protected virtual void DoBuy(Interactor interactor)
	{
		shieldTracker.RefillShields();
		shieldsBought++;
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
        if (shieldTracker == null)
        {
            shieldTracker = interactor.GetComponent<ShieldTracker>();
        }

		if(shieldsBought < costs.Count)
		{
            Cost = costs[shieldsBought];
        }
		else
		{
			Cost = costs[costs.Count - 1];
		}

        if (shieldTracker.CanRefill())
		{
			canBuy = true;
            interactor.display.DisplayMessage(true, buyMessage + " ", "[Cost: " + GetCost(interactor).ToString() + "]");
        }
		else
		{
            canBuy = false;
            interactor.display.DisplayMessage(false, hasMaxShieldText, null);
        }
    }
	protected virtual string GetBuyMessage(Interactor interactor)
	{
		return " to buy " + GetCost(interactor);
	}
	protected virtual int GetCost(Interactor interactor)
	{
		return Cost;
	}

	[ContextMenu("Buy")]
	public void TestBuy()
	{
		DoBuy(FindObjectOfType<Interactor>());
	}
}
