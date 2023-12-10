using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUpgrade : ShopInteractable
{
    SwordStuff swordStuff;
    public string cantBuy;
    [SerializeField] int[] costs;

    private void Awake()
    {
        swordStuff = FindObjectOfType<SwordStuff>();
    }
    protected override void DoBuy(Interactor interactor)
    {
        Ability current = interactor.caster.ActiveAbility;
        AbilityCaster abilityCaster = interactor.caster.caster;
        swordStuff.UpdateMat(interactor.caster.ActiveAbility.tier + 1);

        for (int i = 0; i < abilityCaster.abilities.Length; i++)
        {
            if (i != interactor.caster.activeIndex)
            {
                if (abilityCaster.abilities[i].upgradePath.Enabled)
                {
                    abilityCaster.SetAbility(i,Instantiate(abilityCaster.abilities[i].upgradePath.Value.abilities[abilityCaster.abilities[i].tier + 1]));
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
        int tier = 0;
        AbilityCaster abilityCaster = interactor.caster.caster;
        for (int i = 0; i < abilityCaster.abilities.Length; i++)
        {
            if (abilityCaster.abilities[i].tier > tier)
                tier = abilityCaster.abilities[i].tier;
        }
        if (tier > costs.Length)
        {
            interactor.display.DisplayMessage(false, cantBuy,"");

        }
        else
        interactor.display.DisplayMessage(true, buyMessage + " ", "[Cost: " + GetCost(interactor).ToString() + "]" );

    }


    protected override int GetCost(Interactor interactor)
    {
        AbilityCaster abilityCaster = interactor.caster.caster;
        

        int tier = 0;
        for (int i = 0; i < abilityCaster.abilities.Length; i++)
        {
            if(abilityCaster.abilities[i].tier > tier)
                tier = abilityCaster.abilities[i].tier;
        }
        return costs[Mathf.Min(tier,costs.Length-1)];
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
