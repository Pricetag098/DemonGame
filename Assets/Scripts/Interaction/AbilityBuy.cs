using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbilityBuy : ShopInteractable
{
	
	[Tooltip("Pool of Abilitys to draw from, add nulls to add the disappering feature, try adding multiples of abilitys to change the odds")]
    [SerializeField] List<Ability> abilities = new List<Ability>();
	List<Ability> availablePool = new List<Ability>();
	[SerializeField] bool singleUse;
	[SerializeField] AbilityGiveInteractable giveInteractable;
	[SerializeField] GameObject body;

	GameObject lastPos;
	[SerializeField] List<GameObject> validPositions;
	[SerializeField] Optional<VfxSpawnRequest> vanshFx;
	protected override bool CanBuy(Interactor interactor)
	{
		
		availablePool.Clear();
		
		foreach(Ability ability in abilities)
		{

			if(ability != null)
			{
				if (!interactor.caster.caster.HasAbility(ability))
				{
					availablePool.Add(ability);
				}
			}
			else
			{
				availablePool.Add(null);
			}
		}
		return availablePool.Count > 0;
	}

	protected override void DoBuy(Interactor interactor)
	{
		Ability ability = availablePool[Random.Range(0, availablePool.Count)];
		if(ability == null)
		{
			if(vanshFx.Enabled)
				vanshFx.Value.Play(body.transform.position,body.transform.forward);
			Respawn();
			return;
		}
		giveInteractable.Open(ability);
		
		if (singleUse)
			Disable();
	}

	void Respawn()
	{
		GameObject target = validPositions[Random.Range(0, validPositions.Count)];

		if(lastPos is null)
		{

		}
		else
		{
			lastPos.SetActive(true);
		}
		body.transform.position = target.transform.position;
		body.transform.rotation = target.transform.rotation;
		target.SetActive(false);
		lastPos = target;

	}

	void Disable()
	{
		body.SetActive(false);
	}

}
