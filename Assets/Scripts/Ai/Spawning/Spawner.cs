using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DemonInfo;

public class Spawner : MonoBehaviour
{
    [SerializeField] private bool SpawnerInMap;
    [SerializeField] private bool ParticleOnSpawn;
    [HideInInspector] public Vector3 position;
    [HideInInspector] public bool CanSpawn;
    [HideInInspector] public bool Visited;
    [HideInInspector] public float distToArea;

    public delegate void Action(ObjectPooler spawnParticle);
    private Action particleSpawnAction;

    private void Awake()
    {
        position = transform.position;
        CanSpawn = true;

        if(ParticleOnSpawn == true)
        {
            particleSpawnAction += OnSpawn;
        }
    }

    /// <summary>
    /// Return if a Spawner is able to spawn.
    /// </summary>
    /// <param name="demon"></param>
    /// <param name="spawner"></param>
    /// <param name="sm"></param>
    /// <returns></returns>
    public bool RequestSpawn(DemonType demon, SpawnerManager sm, SpawnType type)
    {
        if(CanSpawn == true)
        {
            SpawnDemon(demon, sm, type);
            particleSpawnAction?.Invoke(sm.ParticleSpawner);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Return if a Spawner is able to spawn for ritual
    /// </summary>
    /// <param name="demon"></param>
    /// <param name="sm"></param>
    /// <param name="list"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool RequestSpawn(DemonType demon, SpawnerManager sm, List<DemonBase> list, SpawnType type)
    {
        if (CanSpawn == true)
        {
            SpawnDemon(demon, sm, list, type);
            particleSpawnAction?.Invoke(sm.ParticleSpawner);
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
    private void SpawnDemon(DemonType demon, SpawnerManager sm, SpawnType type)
    {
        GameObject demonTemp = DemonPoolers.demonPoolers[demon.Id].Spawn();
        DemonBase demonBase = demonTemp.GetComponent<DemonBase>();
        demonBase.SetDemonInMap(SpawnerInMap);
        demonBase.OnSpawn(demon, sm.player, type);
        demonTemp.transform.position = position;
    }

    /// <summary>
    /// Spawns a Demon for a Ritual
    /// </summary>
    /// <param name="demon"></param>
    /// <param name="target"></param>
    /// <param name="list"></param>
    /// <param name="type"></param>
    private void SpawnDemon(DemonType demon, SpawnerManager sm, List<DemonBase> list, SpawnType type)
    {
        GameObject demonTemp = DemonPoolers.demonPoolers[demon.Id].Spawn();
        DemonBase demonBase = demonTemp.GetComponent<DemonBase>();
        demonBase.SetDemonInMap(SpawnerInMap);
        demonBase.OnSpawn(demon, sm.player, type);
        demonTemp.transform.position = position;
        list.Add(demonBase);
    }

    private void OnSpawn(ObjectPooler spawnParticle)
    {
        GameObject obj = spawnParticle.Spawn();
        obj.transform.position = position;
    }
}