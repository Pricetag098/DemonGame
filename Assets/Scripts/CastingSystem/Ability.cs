using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//[CreateAssetMenu(menuName = "Ability")]
public abstract class Ability : ScriptableObject
{
	public string abilityName;
	protected AbilityCaster caster;
	
	
	public virtual void Tick(bool active)
	{

	}

	public void Equip(AbilityCaster abilityCaster)
	{
		caster = abilityCaster;
		OnEquip();
	}

	protected virtual void OnEquip()
	{

	}

	public void DeEquip()
	{

	}

	public virtual void EnableInputs()
	{

	}
	public virtual void DisableInputs()
	{

	}
	

}
