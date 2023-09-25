using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Blessings/BulletHell")]
public class BulletHell : Blessing
{
    public float duration;
	float timer;
	protected override void OnEquip()
	{
		base.OnEquip();
		timer = 0;
		handler.holster.consumeAmmo = false;
	}
	public override void Tick()
	{
		timer += Time.deltaTime;
		if (timer > duration)
			Remove();
	}

	protected override void OnRemove()
	{
		handler.holster.consumeAmmo = true;
	}
}
