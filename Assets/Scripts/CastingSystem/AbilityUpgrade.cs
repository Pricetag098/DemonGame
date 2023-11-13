using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUpgrade : ShopInteractable
{
    protected override void DoBuy(Interactor interactor)
    {
        Ability current = interactor.caster.ActiveAbility;
        AbilityCaster abilityCaster = interactor.GetComponent<AbilityCaster>();
        for (int i = 0; i < abilityCaster.abilities.Length; i++)
        {
            if (abilityCaster.abilities[i] != current)
            {
                if (abilityCaster.abilities[i].upgradePath.Enabled)
                {
                    abilityCaster.abilities[i] = abilityCaster.abilities[i].upgradePath.Value.abilities[abilityCaster.abilities[i].tier + 1];
                }
            }
            else
            {
                interactor.caster.ActiveAbility = current.upgradePath.Value.abilities[current.tier + 1];
            }
        }
        interactor.caster.OnUpgrade();
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
