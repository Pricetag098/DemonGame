using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "abilities/Void walk")]
public class VoidWalk : Ability
{
	[SerializeField] float minCastTime;
	[SerializeField] float moveSpeedBuff,accelerationBuff;
	float castTimer;
	PlayerDeath playerDeath;
	PlayerStats stats;
	bool held = false, startedCasting = false, stoppedCasting = true;
	protected override void OnEquip()
	{
		playerDeath = caster.GetComponent<PlayerDeath>();
		stats = caster.GetComponent<PlayerStats>();
	}

	public override void Cast(Vector3 origin, Vector3 direction)
	{
		if (!startedCasting)
		{
			castTimer = 0;
			startedCasting = true;
			playerDeath.SetWorldState(false);
			stats.accelerationMulti += accelerationBuff;
			stats.speedMulti += moveSpeedBuff;
			stoppedCasting = false;
		}
		
		held = true;
	}

	public override void Tick()
	{
		if (!held && castTimer > minCastTime )
		{
			if (!stoppedCasting)
			{
				stats.accelerationMulti -= accelerationBuff;
				stats.speedMulti -= moveSpeedBuff;
				stoppedCasting = true;
				startedCasting = false;
				playerDeath.SetWorldState(true);
			}
			caster.RemoveBlood(bloodCost * Time.deltaTime);
		}
		else
		{
			castTimer += Time.deltaTime;
			
			held = false;
		}
		

	}
}
