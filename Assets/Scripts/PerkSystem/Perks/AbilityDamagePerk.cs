using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Perks/ADamage")]
public class AbilityDamagePerk : Perk
{
	[SerializeField] float damageModToGain;
	[SerializeField] float killPointModToGain;
	protected override void OnEquip()
	{
		manager.GetComponent<PlayerStats>().abilityDamageMulti += damageModToGain;
	}
	protected override void OnUpgrade()
	{
		GrantPointsOnDeath.gainMod += killPointModToGain;
	}

	protected override void OnUnEquip()
	{
		PlayerStats playerStats = manager.GetComponent<PlayerStats>();
		playerStats.damageMulti -= damageModToGain;
		if (upgraded)
			GrantPointsOnDeath.gainMod -= killPointModToGain;
	}
}
