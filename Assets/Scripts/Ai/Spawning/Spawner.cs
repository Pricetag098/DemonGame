using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [HideInInspector] public Vector3 position;
    [HideInInspector] public bool CanSpawn;
    [HideInInspector] public bool Visited;

    private void Awake()
    {
        position = transform.position;
    }

    /// <summary>
    /// Return if a Spawner is able to spawn.
    /// </summary>
    /// <param name="demon"></param>
    /// <param name="spawner"></param>
    /// <param name="sm"></param>
    /// <returns></returns>
    public bool RequestSpawn(DemonType demon, DemonSpawner spawner, SpawnerManager sm)
    {
        if(CanSpawn == true)
        {
            SpawnDemon(demon, spawner.demonPool, sm.player);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Spawns a Demon
    /// </summary>
    /// <param name="demon"></param>
    /// <param name="pool"></param>
    /// <param name="target"></param>
    private void SpawnDemon(DemonType demon, DemonPoolers pool, Transform target)
    {
        GameObject demonTemp = pool.demonPoolers[demon.Id].Spawn();

        DemonBase demonBase = demonTemp.GetComponent<DemonBase>();

        demonBase.OnSpawn(target);

        demonTemp.transform.position = position;
    }
}