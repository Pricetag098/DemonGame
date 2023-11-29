using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;
[CreateAssetMenu(menuName = "Perks/Speed")]
public class SpeedPerk : Perk
{
	[SerializeField] float speedModToGain;
	[SerializeField] float accelerationModToGain;
	
	protected override void OnEquip()
	{
		PlayerStats playerStats = manager.GetComponent<PlayerStats>();
		playerStats.speedMulti += speedModToGain;
		playerStats.accelerationMulti += accelerationModToGain;
	}
	protected override void OnUpgrade()
	{
		manager.GetComponent<PlayerInputt>().canSprintAndShoot = true;
	}

	protected override void OnUnEquip()
	{
		PlayerStats playerStats = manager.GetComponent<PlayerStats>();
		playerStats.speedMulti -= speedModToGain;
		playerStats.accelerationMulti -= accelerationModToGain;
		if(upgraded)
			manager.GetComponent<PlayerInputt>().canSprintAndShoot = true;

	}
}
