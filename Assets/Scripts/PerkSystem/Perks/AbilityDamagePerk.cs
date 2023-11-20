using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[CreateAssetMenu(menuName = "Perks/ADamage")]
public class AbilityDamagePerk : Perk
{
	[SerializeField] float bloodGainModToGain;
	[SerializeField] float bloodMeterModToGain;

    public TMP_FontAsset perkFont;

    protected override void OnEquip()
	{
		PlayerStats playerStats = manager.GetComponent<PlayerStats>();
		AbilityCaster abilityCaster = manager.GetComponent<AbilityCaster>();

        playerStats.bloodGainMulti = (playerStats.bloodGainMulti * bloodGainModToGain);
		abilityCaster.maxBlood = (abilityCaster.maxBlood * bloodMeterModToGain);
	}
	protected override void OnUpgrade()
	{
		
	}

	protected override void OnUnEquip()
	{
		PlayerStats playerStats = manager.GetComponent<PlayerStats>();
        AbilityCaster abilityCaster = manager.GetComponent<AbilityCaster>();

        playerStats.bloodGainMulti = (playerStats.bloodGainMulti / bloodGainModToGain);
        abilityCaster.maxBlood = (abilityCaster.maxBlood / bloodMeterModToGain);
		if(abilityCaster.blood > abilityCaster.maxBlood)
		{
			abilityCaster.blood = abilityCaster.maxBlood;
		}
    }
}
