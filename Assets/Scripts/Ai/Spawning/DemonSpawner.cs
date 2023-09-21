using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonInfo;
using System.Reflection;
using Unity.Jobs;
using UnityEngine.AI;
//using static UnityEditor.PlayerSettings;

public class DemonSpawner : MonoBehaviour
{
    [HideInInspector] public Queue<DemonType> DemonQueue = new Queue<DemonType>();
    [HideInInspector] public List<DemonBase> ActiveDemons = new List<DemonBase>();

    private Spawners _spawners;
    [HideInInspector] public DemonPoolers demonPool;

    private int baseSpawnerCount = 0;
    private int specialSpawnerCount = 0;

    private void Awake()
    {
        _spawners = GetComponent<Spawners>();
        demonPool = GetComponent<DemonPoolers>();
    }

    /// <summary>
    /// Adds Demon back into Main Queue
    /// </summary>
    /// <param name="demon"></param>
    public void AddDemonBackToPool(DemonType demon, SpawnerManager sm)
    {
        sm.currentDemons--;
        sm.maxDemonsToSpawn++;

        DemonQueue.Enqueue(demon);
    }

    /// <summary>
    /// Gets the ActiveSpawners in Range of the Player
    /// </summary>
    /// <param name="player"></param>
    /// <param name="playerAgent"></param>
    //public void ActiveSpawners(Transform player, NavMeshAgent playerAgent, SpawnerManager sm)
    //{
    //    baseSpawnerCount = _spawners.CheckBaseSpawners(player, playerAgent);
    //    //specialSpawnerCount = _spawners.CheckSpecialSpawners(player, playerAgent);
    //    specialSpawnerCount = 0;
    //}

    public void ActiveSpawners(Areas Id, Areas CurrentArea)
    {
        _spawners.UpdateActiveSpawners(Id, CurrentArea);
    }

    /// <summary>
    /// Despawns All Active Demons
    /// </summary>
    public void DespawnAllActiveDemons()
    {
        int count = ActiveDemons.Count;

        for (int i = 0; i < count; i++)
        {
            ActiveDemons[i].OnDespawn(true);
        }

        ActiveDemons.Clear();
    }

    public void KillAllActiveDemons()
    {
        int count = ActiveDemons.Count;

        for (int i = 0; i < count; i++)
        {
            ActiveDemons[i].ForcedDeath();
        }

        ActiveDemons.Clear();
    }


    /// <summary>
    /// Request a spawner to spawn a Demon returns True if successful
    /// </summary>
    /// <returns></returns>
    public bool SpawnDemon(SpawnerManager sm)
    {
        DemonType demon = null;

        if(DemonCount > 0) { demon = DemonQueue.Dequeue(); }

        switch (demon.SpawnerType)
        {
            case SpawnerType.Basic:
                if (_spawners.baseSpawners.Count > 0)
                {
                    Spawner spawner = null;

                    foreach (Spawner s in _spawners.baseSpawners)
                    {
                        if(s.Visited == false)
                        {
                            s.Visited = true;
                            spawner = s;

                            break;
                        }
                    }

                    if(spawner is null) { DemonQueue.Enqueue(demon); return false; }

                    return spawner.RequestSpawn(demon, this, sm, SpawnType.Default);
                }
                else 
                { 
                    Debug.Log("BASE SPAWNER COUNT 0");
                    DemonQueue.Enqueue(demon);
                }
                break;
            case SpawnerType.Special:
                if (_spawners.specialSpawners.Count > 0)
                {
                    Spawner spawner = null;

                    foreach (Spawner s in _spawners.specialSpawners)
                    {
                        if (s.Visited == false)
                        {
                            s.Visited = true;
                            spawner = s;

                            break;
                        }
                    }

                    if (spawner is null) { DemonQueue.Enqueue(demon); return false; }

                    return spawner.RequestSpawn(demon, this, sm, SpawnType.Default);
                }
                else 
                { 
                    Debug.Log("SPECIAL SPAWNER COUNT 0");
                    DemonQueue.Enqueue(demon);
                }
                break;
            case SpawnerType.Boss:

                break;
        }

        return false;
    }

    public bool SpawnDemonRitual(List<Spawner> spawnPoints, RitualSpawner ritual, SpawnerManager sm, List<DemonBase> list)
    {
        DemonType demon = null;

        if (ritual.DemonCount > 0) { demon = ritual.DemonQueue.Dequeue(); }

        Spawner spawner = null;

        foreach(Spawner s in spawnPoints)
        {
            if (s.Visited == false)
            {
                s.Visited = true;
                spawner = s;

                break;
            }
        }

        if (spawner is null) { ritual.DemonQueue.Enqueue(demon); return false; }

        return spawner.RequestSpawn(demon, this, sm, list, SpawnType.Ritual); ;
    }

    /// <summary>
    /// Returns the Count of the DemonQueue
    /// </summary>
    public int DemonCount
    {
        get { return DemonQueue.Count; }
    }

    public void ResetSpawners()
    {
        _spawners.ResetSpawners();
    }
}
