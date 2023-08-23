using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonInfo;
using System.Reflection;
using Unity.Jobs;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class DemonSpawner : MonoBehaviour
{
    [HideInInspector] public Queue<DemonType> DemonQueue = new Queue<DemonType>();

    private Spawners _spawners;
    [HideInInspector] public DemonPoolers demonPool;

    private int baseSpawnerCount = 0;
    private int specialSpawnerCount = 0;

    private void Awake()
    {
        _spawners = GetComponent<Spawners>();
        demonPool = GetComponent<DemonPoolers>();
    }

    public void AddDemonBackToPool(DemonType demon)
    {
        // minus from current demon count
        // add to max demons to spawn

        //DemonQueue.Enqueue(demon); add back to the pool to spawn from
    }

    public void ActiveSpawners(Transform player, NavMeshAgent playerAgent)
    {
        baseSpawnerCount = _spawners.CheckBaseSpawners(player, playerAgent, this);
        specialSpawnerCount = _spawners.CheckSpecialSpawners(player, playerAgent, this);
    }

    public bool SpawnDemon()
    {
        DemonType demon = null;

        if(DemonCount > 0) { demon = DemonQueue.Dequeue(); }

        if(demon != null)
        {
            switch (demon.SpawnType)
            {
                case SpawnType.Basic:
                    if (baseSpawnerCount > 0)
                    {
                        int temp = Random.Range(0, baseSpawnerCount);
                        Spawner spawner = _spawners.GetBaseSpawner(temp);
                        return spawner.RequestSpawn(demon, this);
                    }
                    else 
                    { 
                        Debug.Log("BASE SPAWNER COUNT 0");
                        DemonQueue.Enqueue(demon);
                    }
                    break;
                case SpawnType.Special:
                    if (specialSpawnerCount > 0)
                    {
                        int temp = Random.Range(0, specialSpawnerCount);
                        Spawner spawner = _spawners.GetSpecialSpawner(temp);
                        return spawner.RequestSpawn(demon, this);
                    }
                    else 
                    { 
                        Debug.Log("SPECIAL SPAWNER COUNT 0");
                        DemonQueue.Enqueue(demon);
                    }
                    break;
                case SpawnType.Boss:

                    break;
            }
        }

        return false;
    }

    public int DemonCount
    {
        get { return DemonQueue.Count; }
    }
}
