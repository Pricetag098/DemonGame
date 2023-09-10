using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxSpawner : MonoBehaviour
{
	public static VfxSpawner instance;
	[SerializeField]
	List<VfxSpawnRequest> vfxlist = new List<VfxSpawnRequest>();

	Dictionary<VfxSpawnRequest, ObjectPooler> poolDict = new Dictionary<VfxSpawnRequest, ObjectPooler>();

	public static SurfaceData DefaultSurfaceData { get { return instance.defaultSurfaceData; } }
	[SerializeField] SurfaceData defaultSurfaceData;
	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("Warning Two VfxSpawners in scene", instance);
			Destroy(instance.gameObject);
		}
		instance = this;
		for (int i = 0; i < vfxlist.Count; i++)
		{
			AddPool(vfxlist[i]);
		}
	}

	public static VfxObject SpawnVfx(VfxSpawnRequest request, Vector3 position, Vector3 direction,Vector3 scale)
	{
		return instance.DoSpawnVfx(request, position, direction, scale);
	}
	public static VfxObject SpawnVfx(int index, Vector3 position, Vector3 direction,Vector3 scale)
	{
		return instance.DoSpawnVfx(instance.vfxlist[index], position, direction, scale);
	}

	void AddPool(VfxSpawnRequest request)
	{
		GameObject go = new GameObject(request.name);
		go.transform.parent = transform;
		ObjectPooler pooler = go.AddComponent<ObjectPooler>();
		pooler.CreatePool(request.prefab, request.poolSize);
		poolDict.Add(request, pooler);
	}

	VfxObject DoSpawnVfx(VfxSpawnRequest request, Vector3 position, Vector3 direction,Vector3 scale)
	{
		if (!vfxlist.Contains(request))
		{
			AddPool(request);
			vfxlist.Add(request);
		}
		GameObject go = poolDict[request].Spawn();
		go.transform.position = position;
		go.transform.forward = direction;
		go.transform.localScale = scale;
		VfxObject vfx = go.GetComponent<VfxObject>();
		vfx.Play();
		return vfx;
	}


}
