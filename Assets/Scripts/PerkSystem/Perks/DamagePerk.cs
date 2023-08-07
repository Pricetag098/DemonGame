using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Perks/Damage")]
public class DamagePerk : Perk
{
	[SerializeField] float damageModToGain;
	[SerializeField] float reloadModToGain;
	protected override void OnEquip()
	{
		manager.GetComponent<PlayerStats>().damageMulti += damageModToGain;
	}
	protected override void OnUpgrade()
	{
		manager.GetComponent<PlayerStats>().reloadTimeMulti += reloadModToGain;
	}

	protected override void OnUnEquip()
	{
		PlayerStats playerStats = manager.GetComponent<PlayerStats>();
		playerStats.damageMulti -= damageModToGain;
		if(upgraded)
		playerStats.reloadTimeMulti -= reloadModToGain;
	}
}
