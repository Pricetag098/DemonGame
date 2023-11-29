using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonInfo;
using System.Reflection;
using Unity.Jobs;
using UnityEngine.AI;
using Assets.CaptainCatSparrow.SpellIconsVolume_2.Druid.Demo;

public class DemonSpawner : MonoBehaviour
{
    [HideInInspector] public Queue<DemonType> DemonQueue = new Queue<DemonType>();
    [HideInInspector] public static List<DemonFramework> ActiveDemons;
    [HideInInspector] public static List<DemonFramework> ActiveDemonsToRemove;

    private const int MAX_DEMON_UPDATES_PER_FRAME = 15;

    private Spawners _spawners;
    [HideInInspector] public DemonPoolers demonPool;

    public int GetDemonQueueCount { get { return DemonQueue.Count; } }
    public int GetActiveDemonCount { get { return ActiveDemons.Count; } }

    private void Awake()
    {
        _spawners = GetComponent<Spawners>();
        demonPool = GetComponent<DemonPoolers>();

        ActiveDemons = new List<DemonFramework>();
        ActiveDemonsToRemove = new List<DemonFramework>();
    }

    public void UpdateCallToDemons()
    {
        int num = ActiveDemons.Count;

        if(num > MAX_DEMON_UPDATES_PER_FRAME) { num = MAX_DEMON_UPDATES_PER_FRAME; }

        for (int i = 0; i < num; i++)
        {
            DemonFramework demon = ActiveDemons[0];

            demon.OnUpdate();

            ActiveDemons.Remove(demon);
            ActiveDemons.Add(demon);
        }
    }

    public void RemoveActiveDemons()
    {
        foreach(DemonFramework demon in ActiveDemonsToRemove)
        {
            ActiveDemons.Remove(demon);
            demon.RemoveFromSpatialHash();
            demon.DespawnObject();
        }

        ActiveDemonsToRemove.Clear();
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

    public void ActiveSpawners(Areas Id, Areas CurrentArea)
    {
        _spawners.UpdateActiveSpawners(Id, CurrentArea);
    }

    /// <summary>
    /// Despawns All Active Demons
    /// </summary>
    public void DespawnAllActiveDemons()
    {
        foreach(DemonFramework d in ActiveDemons)
        {
            d.OnDespawn();
        }
    }

    public void KillAllActiveDemons()
    {
        int count = ActiveDemons.Count;

        for (int i = 0; i < count; i++)
        {
            ActiveDemons[i].OnForcedDeath(true);
        }

        ActiveDemons.Clear();
    }

    public static List<GameObject> AllActiveDemons()
    {
        List<GameObject> list = new List<GameObject>();

        foreach(var demon in ActiveDemons)
        {
            list.Add(demon.gameObject);
        }

        return list;    
    }


    /// <summary>
    /// Request a spawner to spawn a Demon returns True if successful
    /// </summary>
    /// <returns></returns>
    public bool SpawnDemon(SpawnerManager sm)
    {
        DemonType demon = null;

        if(DemonCount > 0) 
        { 
            demon = DemonQueue.Dequeue();

            switch (demon.SpawnerType)
            {
                case SpawnerType.Basic:
                    if (_spawners.baseSpawners.Count > 0)
                    {
                        Spawner spawner = null;

                        foreach (Spawner s in _spawners.baseSpawners)
                        {
                            if (s.Visited == false)
                            {
                                s.Visited = true;
                                spawner = s;

                                break;
                            }
                        }

                        if (spawner == null) { DemonQueue.Enqueue(demon); return false; }

                        return spawner.RequestSpawn(demon, sm, SpawnType.Default);
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

                        if (spawner == null) { DemonQueue.Enqueue(demon); return false; }

                        return spawner.RequestSpawn(demon, sm, SpawnType.Default);
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
        }

        

        return false;
    }

    public bool SpawnDemonRitual(List<Spawner> spawnPoints, RitualSpawner ritual, SpawnerManager sm, List<DemonFramework> list)
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

        if (spawner == null) { ritual.DemonQueue.Enqueue(demon); return false; }

        return spawner.RequestSpawn(demon, sm, list, SpawnType.Ritual); ;
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
