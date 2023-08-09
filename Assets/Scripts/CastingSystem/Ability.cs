using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[CreateAssetMenu(menuName = "abilities/Empty")]
public class Ability : ScriptableObject
{
	public string abilityName;
	public Sprite icon;
	protected AbilityCaster caster;
	public float bloodCost = 10;
	public enum CastModes
	{
		press,
		hold,
		passive
	}
	public CastModes castMode;
	public virtual void Tick()
	{

	}

	public void Equip(AbilityCaster abilityCaster)
	{
		caster = abilityCaster;
		OnEquip();
	}

	public virtual void Cast(Vector3 origin,Vector3 direction)
	{

	}

	protected virtual void OnEquip()
	{

	}

	public void DeEquip()
	{
		OnDeEquip();	
	}
    protected virtual void OnDeEquip()
    {

    }


}
