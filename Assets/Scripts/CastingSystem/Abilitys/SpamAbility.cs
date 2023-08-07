using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="old blood knife")]
public class SpamAbility : Ability
{
    [SerializeField] float damage;
	[SerializeField] GameObject prefab;
	ObjectPooler pool;
	[SerializeField] int poolSize;
	[SerializeField] float spreadUnits;
	[SerializeField] float castsPerMin;
	[SerializeField] float speed;
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
				pool.Spawn().GetComponent<DamageProjectiles>().Shoot(origin + Random.insideUnitSphere * spreadUnits, direction * speed, damage, caster.castOrigin, 1);
				timer = 0;
				caster.blood -= bloodCost;
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
}
