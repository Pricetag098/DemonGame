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
		//for(int i = DemonSpawner.ActiveDemons.Count; i > 0; i--)
		//{
		//	//DemonSpawner.ActiveDemons[i].GetComponent<Health>().TakeDmg(1000000);
		//	DemonSpawner.ActiveDemons[i].ForcedDeath();
		//}

		foreach(DemonFramework d in DemonSpawner.ActiveDemons)
		{
			d.OnForcedDeath(false);
		}

		DemonSpawner.ActiveDemons.Clear();
	}
}
