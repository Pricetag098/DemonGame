using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkBuy : ShopInteractable
{
	public bool upgraded;
	[SerializeField] public Perk perk;
	public Material upgradedMat;
	[SerializeField] GameObject chalice;

	PerkNotification perkNotification;

    private void Awake()
    {
        perkNotification = FindObjectOfType<PerkNotification>();
    }

    protected override void DoBuy(Interactor interactor)
	{
		interactor.perkManager.AddPerk(Instantiate(perk));
		if(upgraded)
		{
			Upgrade(interactor.perkManager);
        }
        perkNotification.Notify(perk);
    }

    protected override bool CanBuy(Interactor interactor)
	{
		return !interactor.perkManager.HasPerk(perk);
	}

    public override void EndHover(Interactor interactor)
    {
        base.EndHover(interactor);
        interactor.display.HideText();
    }
    public override void StartHover(Interactor interactor)
    {
        base.StartHover(interactor);
		interactor.display.DisplayMessage(true, " " + buyMessage + " " + perk.perkName + " ", "[Cost: " + GetCost(interactor).ToString() + "]" ); 
    }

    public void Upgrade(PerkManager perkManager)
	{
		if(perkManager.HasPerk(perk))
		{
			perkManager.GetPerk(perk).Upgrade();
			upgraded = true;
		}

		foreach(Transform child in chalice.transform)
		{
			child.GetComponent<Renderer>().material = upgradedMat;
		}
	}
	[ContextMenu("Upgrade")]
	void TestUpgrade()
	{
		Upgrade(FindObjectOfType<PerkManager>());
	}

	

}
