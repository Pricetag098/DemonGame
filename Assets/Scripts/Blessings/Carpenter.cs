using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Blessings/Carpenter")]
public class Carpenter : Blessing
{
	protected override void OnEquip()
	{
		foreach(DestrcutibleObject destrcutibleObject in handler.destrcutibleObjects)
		{
			destrcutibleObject.RestoreHealthToMax();
		}
	}
}
