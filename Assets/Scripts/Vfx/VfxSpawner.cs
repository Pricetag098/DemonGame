using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxSpawner : MonoBehaviour
{
    public static VfxSpawner instance;
	[SerializeField]
	List<ObjectPooler> vfxPools = new List<ObjectPooler>();
	private void Awake()
	{
		if (instance != null)
			Debug.LogWarning("Warning Two VfxSpawners in scene", instance);
		instance = this;
	}

	public static void SpawnVfx(int index, Vector3 position, Vector3 direction)
	{
		instance.DoSpawnVfx(index, position, direction);
	}

	void DoSpawnVfx(int index,Vector3 position,Vector3 direction)
	{
		GameObject go = vfxPools[index].Spawn();
		go.transform.position = position;
		go.transform.up = direction;
		go.GetComponent<VfxObject>().Play();
	}

	
}
