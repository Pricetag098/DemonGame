using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[CreateAssetMenu(menuName = "abilities/Empty")]
public class Ability : ScriptableObject
{
	public string abilityName;
	public string guid;
	public Sprite icon;
	[HideInInspector]
	public AbilityCaster caster;
	public float bloodCost = 10;
	public Optional<AbilityUpgradePath> upgradePath;
	public int tier = 0;
	public int abilitySlot;
	public int fontReference;
	public float killPointMod;

	public float drawTime = 1,holsterTime = 1;

	public RuntimeAnimatorController controller;
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
	public virtual void EndSelect()
	{

	}
	
	public virtual void StartSelect()
	{

	}

    
    public virtual void EndDeSelect()
    {

    }

	public virtual void StartDeSelect()
	{

	}
	public virtual void OnHit(Health health)
	{

	}
	[ContextMenu("Gen Guid")]
	void GenGuid()
	{
		guid = System.Guid.NewGuid().ToString();
	}

}
