using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Blessings/InstaKill")]
public class InstaKill : Blessing
{
	[Tooltip("adds to the modifier 1 would double")]
	public float modifier;
	public float duration;
	float timer;
	protected override void OnEquip()
	{
		timer = 0;
		handler.playerStats.damageMulti += modifier;
		handler.playerStats.abilityDamageMulti += modifier;
	}
	public override void Tick()
	{
		timer += Time.deltaTime;
		if (timer > duration)
			Remove();
	}

	protected override void OnRemove()
	{
		handler.playerStats.damageMulti -= modifier;
		handler.playerStats.abilityDamageMulti -= modifier;
	}
}
