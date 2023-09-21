using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DemonInfo;

public class Spawner : MonoBehaviour
{
    [HideInInspector] public Vector3 position;
    [HideInInspector] public bool CanSpawn;
    [HideInInspector] public bool Visited;
    [HideInInspector] public float distToArea;

    private void Awake()
    {
        position = transform.position;
        CanSpawn = true;
    }

    /// <summary>
    /// Return if a Spawner is able to spawn.
    /// </summary>
    /// <param name="demon"></param>
    /// <param name="spawner"></param>
    /// <param name="sm"></param>
    /// <returns></returns>
    public bool RequestSpawn(DemonType demon, DemonSpawner spawner, SpawnerManager sm, SpawnType type)
    {
        if(CanSpawn == true)
        {
            SpawnDemon(demon, spawner.demonPool, sm.player, type);

            return true;
        }

        return false;
    }
    public bool RequestSpawn(DemonType demon, DemonSpawner spawner, SpawnerManager sm, List<DemonBase> list, SpawnType type)
    {
        if (CanSpawn == true)
        {
            SpawnDemon(demon, spawner.demonPool, sm.player, list, type);

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
    private void SpawnDemon(DemonType demon, DemonPoolers pool, Transform target, SpawnType type)
    {
        GameObject demonTemp = pool.demonPoolers[demon.Id].Spawn();
        DemonBase demonBase = demonTemp.GetComponent<DemonBase>();
        demonBase.setSpawnPosition(position);
        demonBase.OnSpawn(demon, target, type);
        demonTemp.transform.position = position;
    }

    private void SpawnDemon(DemonType demon, DemonPoolers pool, Transform target, List<DemonBase> list, SpawnType type)
    {
        GameObject demonTemp = pool.demonPoolers[demon.Id].Spawn();
        DemonBase demonBase = demonTemp.GetComponent<DemonBase>();
        demonBase.setSpawnPosition(position);
        demonBase.OnSpawn(demon, target, type);
        demonTemp.transform.position = position;
        list.Add(demonBase);
    }
}