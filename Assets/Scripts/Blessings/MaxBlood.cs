using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Blessings/MaxBlood")]
public class MaxBlood : Blessing
{
	protected override void OnEquip()
	{
		handler.abilityCaster.AddBlood(handler.abilityCaster.maxBlood - handler.abilityCaster.blood);
	}
}
