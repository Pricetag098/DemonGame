using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//[CreateAssetMenu(menuName = "Ability")]
public abstract class Ability : ScriptableObject
{
	public string abilityName;
	protected AbilityCaster caster;
	
	public enum CastModes
	{
		press,
		hold,
		passive
	}
	public CastModes castMode;
	public virtual void Tick(bool active)
	{

	}

	public void Equip(AbilityCaster abilityCaster)
	{
		caster = abilityCaster;
		OnEquip();
	}

	public virtual void Cast()
	{

	}

	protected virtual void OnEquip()
	{

	}

	public void DeEquip()
	{

	}

	

}
