using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Blessings/Nuke")]
public class Nuke : Blessing
{
	[SerializeField] VfxSpawnRequest vfx;
	protected override void OnEquip()
	{
		//todo: loop through all enemys and deal a million damage
		foreach(DemonBase g in DemonSpawner.ActiveDemons)
		{
			g.GetComponent<Health>().TakeDmg(1000000);
		}
	}
}
