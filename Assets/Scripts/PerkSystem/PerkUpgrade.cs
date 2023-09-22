using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkUpgrade : Interactable
{
    private Perk perk;
    public PerkBuy perkBuy;
    public string interactMessage;

    private void Awake()
    {
        perk = perkBuy.perk;
    }

    public override void Interact(Interactor interactor)
    {
        PerkManager perkManager = interactor.gameObject.GetComponent<PerkManager>();

        if (perkManager.HasPerk(perk))
        {
            Perk playerPerk = perkManager.GetPerk(perk);
            if(playerPerk != null)
            {
                playerPerk.upgraded = true;
            }
        }

        perk.upgraded = true;
    }

    public override void StartHover(Interactor interactor)
    {
        base.StartHover(interactor);
        interactor.display.DisplayMessage(false, interactMessage);
    }

    public override void EndHover(Interactor interactor)
    {
        base.EndHover(interactor);
        interactor.display.HideText();
    }
}
