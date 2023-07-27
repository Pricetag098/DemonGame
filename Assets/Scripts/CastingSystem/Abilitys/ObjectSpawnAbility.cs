using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//[CreateAssetMenu(menuName = "Abilitys/BloodBolt")]
public class ObjectSpawnAbility : Ability
{
	
	public GameObject prefabToSpawn;

	public float castsPerMin = 100;
	public float projectileSpeed = 300;
	public float damage = 10;
	public float bloodCost = 10;
	public float spawnOffset = 1; // will look into a raycast for this later



	[Min(1)]
	public int poolSize = 10;
	float timer;
	ObjectPooler pool;

	
	
	// Start is called before the first frame update
	public override void Tick(bool active)
	{
		
		timer -= Time.deltaTime;
	}



	

	protected override void OnEquip()
	{
		pool = new GameObject(abilityName + "Pool").AddComponent<ObjectPooler>();
		pool.CreatePool(prefabToSpawn, poolSize);
	}
	private void OnDestroy()
	{
		Destroy(pool);
	}

	public override void Cast()
	{
		if (timer < 0 && caster.blood >= bloodCost)
		{
			Fire();
			timer = 1 / (castsPerMin / 60);
			caster.blood -= bloodCost;
		}
	}

	protected virtual void Fire()
	{
		
			GameObject projectile = pool.Spawn();
			//Debug.Log(projectile != null);
			projectile.GetComponent<DamageProjectiles>().Shoot(Camera.main.transform.position, Camera.main.transform.forward * projectileSpeed, damage, caster.castOrigin);
		
	}
}
