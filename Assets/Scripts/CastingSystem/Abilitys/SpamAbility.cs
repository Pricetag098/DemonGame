using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "abilities/old blood knife")]
public class SpamAbility : Ability
{
    [SerializeField] float damage;
	[SerializeField] GameObject prefab;
	ObjectPooler pool;
	[SerializeField] int poolSize;
	[SerializeField] int points;
	[SerializeField] float spreadUnits;
	[SerializeField] float castsPerMin;
	[SerializeField] float speed;
	[SerializeField] VfxSpawnRequest spawnFx;
	float cooldown;
	float timer;
	public override void Tick()
	{
		timer += Time.deltaTime;
	}
	public override void Cast(Vector3 origin, Vector3 direction)
	{
		if(timer > cooldown)
		{
			if(caster.blood > bloodCost)
			{
				pool.Spawn().GetComponent<DamageProjectiles>().Shoot(origin + Random.insideUnitSphere * spreadUnits, direction * speed, damage, this, 1);
				timer = 0;
				caster.RemoveBlood(bloodCost);
				spawnFx.Play(caster.castOrigin.position,direction);
			}
			
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

	public override void OnHit()
	{
		if (caster.playerStats.Enabled)
			caster.playerStats.Value.GainPoints(points);
	}
}
