using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBuy : ShopInteractable
{
	[SerializeField] PlayerAbilityCaster caster;
	[Tooltip("Pool of Abilitys to draw from, add nulls to add the disappering feature, try adding multiples of abilitys to change the odds")]
    [SerializeField] List<Ability> abilities = new List<Ability>();
	List<Ability> availablePool = new List<Ability>();
	[SerializeField] bool singleUse;
	protected override bool CanBuy()
	{
		
		availablePool.Clear();
		
		foreach(Ability ability in abilities)
		{

			if(ability != null)
			{
				if (!caster.caster.HasAbility(ability))
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

	protected override void DoBuy()
	{
		Ability ability = availablePool[Random.Range(0, availablePool.Count)];
		if(ability == null)
		{
			Respawn();
			return;
		}
		
		caster.caster.SetAbility(caster.activeIndex, Instantiate(ability));
		if (singleUse)
			Disable();
	}

	void Respawn()
	{
		transform.parent.gameObject.SetActive(false);
	}

	void Disable()
	{
		transform.parent.gameObject.SetActive(false);
	}

}
