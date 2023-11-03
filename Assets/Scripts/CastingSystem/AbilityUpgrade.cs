using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUpgrade : ShopInteractable
{
    protected override void DoBuy(Interactor interactor)
    {
        Ability current = interactor.caster.ActiveAbility;
        interactor.caster.ActiveAbility = current.upgradePath.Value.abilities[current.tier + 1];
    }

    public override void StartHover(Interactor interactor)
    {
        Ability current = interactor.caster.ActiveAbility;
        base.StartHover(interactor);
        interactor.display.DisplayMessage(true, buyMessage + " " + current.abilityName + ": " + GetCost(interactor));

    }


    protected override bool CanBuy(Interactor interactor)
    {
        if (interactor.caster.ActiveAbility.upgradePath.Enabled)
        {
            if (interactor.caster.ActiveAbility.upgradePath.Value.abilities.Count > interactor.caster.ActiveAbility.tier +1)
            {
                return true;
            }
        }
        return false;
    }
}
