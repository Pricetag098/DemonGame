using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Perks/BloodGain")]
public class BloodGainPerk : Perk
{
	[SerializeField] float bloodPerSecond;
	AbilityCaster caster;
	protected override void OnEquip()
	{
		manager.tickPerk += Tick;
		caster = manager.GetComponent<AbilityCaster>();
	}
	protected override void OnUpgrade()
	{
		//Do Later when ai is further along
	}
	void Tick()
	{
		caster.AddBlood(bloodPerSecond * Time.deltaTime);
	}

	protected override void OnUnEquip()
	{
		manager.tickPerk -= Tick;
	}
}
