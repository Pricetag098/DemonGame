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
	[SerializeField] AimAssist aimAssist;

	bool startedCasting;
	bool pressed;
	
	public override void Tick()
	{
		timer += Time.deltaTime;
		if (!pressed && startedCasting)
		{
			caster.animator.SetBool("Held", false);
			startedCasting = false;
		}
		pressed = false;
	}
	public override void Cast(Vector3 origin, Vector3 direction)
	{
		if(!startedCasting)
		{
			caster.animator.SetTrigger("Cast");
			caster.animator.SetBool("Held", true);
			startedCasting = true;
		}
		if(timer > cooldown)
		{
			Vector3 rand = Random.insideUnitSphere * spreadUnits;
			if (aimAssist.GetAssistedAimDir(direction,origin,1,out Transform target,new List<Health>()))
			{
				float d = Vector3.Distance(origin, target.position);
				Vector3 mid = origin + (direction * d / 2) ;

				pool.Spawn().GetComponent<DamageProjectiles>().Shoot(origin + rand, mid + rand,target,rand, d/speed, damage, this, 1);
			}
			else
			{
				Vector3 end = origin + direction * 100;
				Vector3 mid = Vector3.Lerp(origin, end, .5f);
				pool.Spawn().GetComponent<DamageProjectiles>().Shoot(origin + rand, mid + rand,end + rand,100/speed, damage, this, 1);
			}


			
			timer = 0;
			caster.RemoveBlood(bloodCost);
			spawnFx.Play(caster.castOrigin.position,direction);
			
			
		}
		pressed = true;
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
        if (health.dead)
            return;
        if (caster.playerStats.Enabled)
			caster.playerStats.Value.GainPoints(points);
	}
}
