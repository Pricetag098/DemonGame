using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class AbilityGiveInteractable : Interactable
{
	[SerializeField] float lifeTime;
	float timer;
	public Ability ability;
	[SerializeField] AbilityBuy abilityBuy;
	public TextMeshProUGUI icon;
	[SerializeField] string grabText = "To take ";
	[SerializeField] AnimationCurve alphaOverLifetime;
	CanvasGroup canvasGroup;
	private void Awake()
	{
		canvasGroup = icon.GetComponent<CanvasGroup>();
	}
/*	public override void Interact(Interactor interactor)
	{
		interactor.caster.SetAbility(Instantiate(ability));
		Close();
		
	}*/

	public override void StartHover(Interactor interactor)
	{
		base.StartHover(interactor);
		interactor.display.DisplayMessage(true, grabText + ability.abilityName, null);

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
		icon.text = ability.symbolText;
		abilityBuy.gameObject.SetActive(false);
		timer = 0;
	}
	public void Close()
	{
		gameObject.SetActive(false);
		abilityBuy.gameObject.SetActive(true);
	}

	private void Update()
	{
		if(timer > lifeTime)
		{
			Close();
		}
		canvasGroup.alpha = alphaOverLifetime.Evaluate(timer / lifeTime);
		timer += Time.deltaTime;
	}
}
