using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [Tooltip("The prefab to spawn")]
    public GameObject prefabToPool;
    [Min(1)] [Tooltip("How many objects in the pool")]
    public int poolSize = 1;
    [Tooltip("If enabled the pool will refill when empty otherwise will despawn the oldest alive object")]
    [SerializeField] bool autoRefillPool = false;
    [Tooltip("Where to place the spawned objects in the hierachy, if left empy one will be spawned for you")]
    [SerializeField] Transform poolContainer;

    Queue<PooledObject> pool = new Queue<PooledObject>();
    List<PooledObject> activeObjects = new List<PooledObject>();

    // Start is called before the first frame update
    void Awake()
    {
        if (prefabToPool is null)
            return;
        if(poolContainer is null)
		{
            poolContainer = new GameObject().transform;
            poolContainer.name = gameObject.name + ": Pool";
		}
        FillPool();
    }

    public void CreatePool(GameObject prefab, int size)
	{
        prefabToPool = prefab;
        poolSize = size;
        poolContainer = transform;
        FillPool();
	}
   
    /// <summary>
    /// Grabs an object from the pool and adds it to the active list
    /// </summary>
    /// <returns>the requested object</returns>
    public GameObject Spawn()
	{
        PooledObject obj;
        if(pool.TryDequeue(out obj))
		{
            activeObjects.Add(obj);
            obj.gameObject.SetActive(true);
            return obj.gameObject;
		}
		else
		{
            RefillPool();
            return Spawn();
		}
	}

    /// <summary>
    /// Returns an object to the pool and disables it
    /// </summary>
    /// <param name="obj">the object to despawn</param>
    public void Despawn(PooledObject obj)
	{
        activeObjects.Remove(obj);
        pool.Enqueue(obj);
        obj.gameObject.SetActive(false);
	}

    void DespawnOldest()
	{
        if(activeObjects.Count > 0)
		{
            Despawn(activeObjects[0]);
		}
	}

    void FillPool()
	{
        for(int i = 0; i < poolSize; i++)
		{
            InstansiatePooledObject();
		}
	}

    void InstansiatePooledObject()
	{
        GameObject spawnedGo = Instantiate(prefabToPool, poolContainer);
        PooledObject pooledObject = spawnedGo.GetComponent<PooledObject>();
        if(pooledObject == null)
		{
            pooledObject = spawnedGo.AddComponent<PooledObject>();
            
		}
        pooledObject.origin = this;
        spawnedGo.SetActive(false);
        pool.Enqueue(pooledObject);
	}

    public void DespawnAll()
	{
        for(int i = activeObjects.Count - 1; i >= 0; i--)
		{
            activeObjects[i].Despawn();
		}
	}

    void RefillPool()
	{
		if (autoRefillPool)
		{
            FillPool();
            poolSize += poolSize;
		}
		else
		{
            DespawnOldest();
		}
	}
	private void OnDestroy()
	{
        Destroy(poolContainer.gameObject);
	}


}
