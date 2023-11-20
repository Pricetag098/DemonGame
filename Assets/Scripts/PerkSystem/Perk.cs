using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk : ScriptableObject
{
	protected PerkManager manager;
	public string symbolText;
	public string perkName;
	public bool upgraded = false;
    public void Equip(PerkManager perkManager)
	{
		manager = perkManager;
		OnEquip();
	}

	protected virtual void OnEquip()
	{

	}

	public void Upgrade()
	{
		if (upgraded)
			return;
		upgraded = true;
		OnUpgrade();
	}
	protected virtual void OnUpgrade()
	{

	}

	public void UnEquip()
	{ 
		OnUnEquip();
	}

	protected virtual void OnUnEquip()
	{

	}
}
