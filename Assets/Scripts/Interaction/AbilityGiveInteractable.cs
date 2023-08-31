using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AbilityGiveInteractable : Interactable
{

	public Ability ability;
	[SerializeField] AbilityBuy abilityBuy;
	public Image icon;
	[SerializeField] string grabText = "To take ";
	public override void Interact(Interactor interactor)
	{
		interactor.caster.SetAbility(Instantiate(ability));
	}

	public override void StartHover(Interactor interactor)
	{
		base.StartHover(interactor);
		interactor.display.DisplayMessage(true, grabText + ability.abilityName);
	}

	public override void EndHover(Interactor interactor)
	{
		base.EndHover(interactor);
		interactor.display.HideText();
	}

	public void Open(Ability ability)
	{
		this.ability = ability;
		gameObject.SetActive(true);
		icon.sprite = ability.icon;
		abilityBuy.enabled = false;
	}
	public void Close()
	{
		gameObject.SetActive(false);
		abilityBuy.enabled = true;
	}
}
