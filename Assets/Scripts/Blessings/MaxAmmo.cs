using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Blessings/MaxAmmo")]
public class MaxAmmo : Blessing
{
	protected override void OnEquip()
	{
		foreach(Gun gun in handler.holster.guns)
		{
			if(gun is not null)
			{
                gun.RefillStash();
            }
		}
	}
}
