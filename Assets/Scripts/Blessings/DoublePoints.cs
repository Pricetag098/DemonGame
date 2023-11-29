using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Blessings/DoublePoints")]
public class DoublePoints : Blessing
{
	[Tooltip("adds to the modifier 1 would double")]
	public float modifier;
	public float duration;
	float timer;
	protected override void OnEquip()
	{
		timer = 0;
		handler.playerStats.pointGainMulti += modifier;
	}
	public override void Tick()
	{
		timer += Time.deltaTime;
		if (timer > duration)
			Remove();
	}
	public override void ReEquip()
	{
		timer = 0;
	}
	protected override void OnRemove()
	{
		handler.playerStats.pointGainMulti -= modifier;
	}
}
