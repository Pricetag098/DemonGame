using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessingSpawner : MonoBehaviour
{
    public List<ObjectPooler> poolers;
    private List<ObjectPooler> activePoolers;

    private ObjectPooler lastPooler;

    private void Awake()
    {
        foreach(Transform t in transform)
        {
            if(t.TryGetComponent<ObjectPooler>(out ObjectPooler pool))
            {
                poolers.Add(pool);
            }
        }
    }

    public void SpawnBlessing(Transform spawnPos)
    {
        int ran = Random.Range(0, activePoolers.Count - 1);
        lastPooler = poolers[ran];
        GameObject blessing = lastPooler.Spawn();
        blessing.transform.position = spawnPos.position;

        activePoolers.Clear();
        activePoolers.AddRange(poolers);

        activePoolers.Remove(lastPooler);
    }


}
