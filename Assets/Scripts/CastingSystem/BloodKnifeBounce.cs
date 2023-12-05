using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodKnifeBounce : MonoBehaviour
{
	DamageProjectiles damageProjectiles;
	[SerializeField] AimAssist aimAssist;
	List<Health> hits = new List<Health>();
	[SerializeField] float postBounceSpeed = 100;
	private void Awake()
	{
		damageProjectiles = GetComponent<DamageProjectiles>();
		damageProjectiles.onPenetrate += Bounce;
		GetComponent<PooledObject>().OnDespawn += Despawn;
	}

	void Despawn()
	{
		hits.Clear();
	}
	void Bounce(Health health)
	{
		hits.Add(health);
		Transform target;
		if (aimAssist.GetAssistedAimDir(transform.forward, transform.position, 1, out target, hits))
		{
			float distance = Vector3.Distance(transform.position, target.position);
			Vector3 mid = transform.position + distance * .5f * transform.forward;
			
			damageProjectiles.Shoot(transform.position, mid, target,damageProjectiles.offset, distance / postBounceSpeed,damageProjectiles.damage, damageProjectiles.ability,damageProjectiles.maxPenetrations - damageProjectiles.penetrations,0);
		}
	}
}
