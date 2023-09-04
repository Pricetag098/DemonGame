using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    public bool RequestSpawn(DemonType demon, DemonSpawner spawner, SpawnerManager sm, bool defaultSpawn = true)
    {
        if(CanSpawn == true)
        {
            SpawnDemon(demon, spawner.demonPool, sm.player, defaultSpawn);

            return true;
        }

        return false;
    }
    public bool RequestSpawn(DemonType demon, DemonSpawner spawner, SpawnerManager sm, List<DemonBase> list, bool defaultSpawn = true)
    {
        if (CanSpawn == true)
        {
            SpawnDemon(demon, spawner.demonPool, sm.player, list, defaultSpawn);

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
    private void SpawnDemon(DemonType demon, DemonPoolers pool, Transform target, bool defaultSpawn = true)
    {
        GameObject demonTemp = pool.demonPoolers[demon.Id].Spawn();
        demonTemp.transform.position = position;
        DemonBase demonBase = demonTemp.GetComponent<DemonBase>();
        demonBase.OnSpawn(target, defaultSpawn);
    }

    private void SpawnDemon(DemonType demon, DemonPoolers pool, Transform target, List<DemonBase> list, bool defaultSpawn = true)
    {
        GameObject demonTemp = pool.demonPoolers[demon.Id].Spawn();
        demonTemp.transform.position = position;
        DemonBase demonBase = demonTemp.GetComponent<DemonBase>();
        demonBase.OnSpawn(target, defaultSpawn);
        list.Add(demonBase);
    }
}