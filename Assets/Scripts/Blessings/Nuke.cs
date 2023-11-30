using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Blessings/Nuke")]
public class Nuke : Blessing
{
	[SerializeField] VfxSpawnRequest vfx;
	protected override void OnEquip()
	{
		foreach(DemonFramework d in DemonSpawner.ActiveDemons)
		{
			d.OnForcedDeath(false);
		}

		DemonSpawner.ActiveDemons.Clear();
	}
}