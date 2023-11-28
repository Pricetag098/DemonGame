using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "abilities/FingerGun")]
public class FingerGun : Ability
{
	[SerializeField] float damage;
	[SerializeField] GameObject prefab;
	ObjectPooler pool;
	[SerializeField] int poolSize;
	[SerializeField] int points;
	[SerializeField] float spreadUnits;
	[SerializeField] float castsPerMin;
	[SerializeField] float speed;
	[SerializeField] VfxSpawnRequest spawnFx,executeFx;
	[Range(0f, 1f)]
	[SerializeField] float executePercent;
	float cooldown;
	float timer;
	[SerializeField] AimAssist aimAssist;

	public override void Tick(Vector3 origin, Vector3 direction)
	{
		timer += Time.deltaTime;
	}
	public override void Cast(Vector3 origin, Vector3 direction)
	{
		if (timer > cooldown)
		{
			Vector3 rand = Random.insideUnitSphere * spreadUnits;
			Vector3 end = origin + direction * 100;
			Vector3 mid = Vector3.Lerp(origin, end, .5f);
			pool.Spawn().GetComponent<DamageProjectiles>().Shoot(origin + rand, mid + rand, end + rand, 100 / speed, damage * caster.DamageMulti, this, 1);

			timer = 0;
			caster.RemoveBlood(bloodCost);
			spawnFx.Play(caster.castOrigin.position, direction);


		}

	}

	protected override void OnEquip()
	{
		pool = new GameObject().AddComponent<ObjectPooler>();
		pool.CreatePool(prefab, poolSize);
		cooldown = 1 / (castsPerMin / 60);
	}

	protected override void OnDeEquip()
	{
		Destroy(pool.gameObject);
	}

	public override void OnHit(Health health)
	{
		if (caster.playerStats.Enabled)
			caster.playerStats.Value.GainPoints(points);

		if(health.health / health.maxHealth < executePercent)
		{
			health.TakeDmg(float.PositiveInfinity, HitType.ABILITY);
			if(health.TryGetComponent(out VfxTargets target))
			{
				executeFx.Play(target.origin.position, target.origin.forward);
			}
		}
	}
}
