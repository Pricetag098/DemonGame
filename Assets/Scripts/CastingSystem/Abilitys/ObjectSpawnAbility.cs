using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//[CreateAssetMenu(menuName = "Abilitys/BloodBolt")]
public class ObjectSpawnAbility : Ability
{
	public InputActionProperty useAction;
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

	public bool hold;
	
	// Start is called before the first frame update
	public override void Tick(bool active)
	{
		if (active)
		{
			if ((useAction.action.WasPressedThisFrame() && !hold) || (useAction.action.IsPressed() && hold))
			{
				if(timer < 0 && caster.blood >= bloodCost)
				{
					timer = 1 / (castsPerMin / 60);
					caster.blood -=bloodCost;
					Cast();
					
				}
			}
		}
		timer -= Time.deltaTime;
	}



	private void OnEnable()
	{
		useAction.action.Enable();
	}

	private void OnDisable()
	{
		useAction.action.Disable();
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

	protected virtual void Cast()
	{
		GameObject projectile = pool.Spawn();
		Debug.Log(projectile != null);
		projectile.GetComponent<DamageProjectiles>().Shoot(Camera.main.transform.position + Camera.main.transform.forward * spawnOffset, Camera.main.transform.forward * projectileSpeed,damage);
	}
}
