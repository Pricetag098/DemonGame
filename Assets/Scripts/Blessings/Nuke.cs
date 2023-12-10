using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Blessings/Nuke")]
public class Nuke : Blessing
{
	[SerializeField] VfxSpawnRequest vfx;
	[SerializeField] int pointsToGain = 500;
	protected override void OnEquip()
	{
		handler.playerStats.GainPoints(pointsToGain);
		foreach(DemonFramework d in DemonSpawner.ActiveDemons)
		{
			d.OnForcedDeath(false);
		}

		//DemonSpawner.ActiveDemons.Clear();
	}
}