using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Spawner : MonoBehaviour
{
    public Vector3 position;

    private SpawnerManager sm;
    private DemonSpawner demonSpawner;
    private Transform player;
    private float timeBetweenSpawns;
    private float spawnTimer;

    [HideInInspector] public bool CanSpawn;
    [HideInInspector] public bool Visited;

    public Queue<DemonType> demonsToSpawn = new Queue<DemonType>();

    private void Awake()
    {
        demonSpawner = FindObjectOfType<DemonSpawner>();
        sm = FindObjectOfType<SpawnerManager>();
        //player = sm.player;
        //timeBetweenSpawns = sm.timeBetweenSpawns;

        position = transform.position;
    }

    //private void Update()
    //{
    //    spawnTimer += Time.deltaTime;

    //    if(HelperFuntions.GreaterThanOrEqual(spawnTimer, timeBetweenSpawns))
    //    {
    //        if(demonsToSpawn.Count > 0 && CanSpawn == true)
    //        {
    //            SpawnDemon(demonsToSpawn.Dequeue(), demonSpawner.demonPool, player);

    //            sm.currentDemons++;
    //            sm.maxDemonsToSpawn--;

    //            spawnTimer = 0;
    //        }
    //    }
    //}

    public bool RequestSpawn(DemonType demon, DemonSpawner spawner, SpawnerManager sm)
    {
        if(CanSpawn == true)
        {
            SpawnDemon(demon, spawner.demonPool, sm.player);
            sm.currentDemons++;
            sm.maxDemonsToSpawn--;

            return true;
        }

        return false;
    }

    //public bool RequestSpawn(DemonType demon, DemonSpawner spawner)
    //{
    //    if (demonsToSpawn.Count < 5)
    //    {
    //        demonsToSpawn.Enqueue(demon); // spawns demon
    //        return true;
    //    }

    //    spawner.DemonQueue.Enqueue(demon); // returns demon to queue
    //    return false;
    //}

    private void SpawnDemon(DemonType demon, DemonPoolers pool, Transform target)
    {
        GameObject demonTemp = pool.demonPoolers[demon.Id].Spawn();

        DemonBase demonBase = demonTemp.GetComponent<DemonBase>();

        demonBase.OnSpawn(target);

        demonTemp.transform.position = position;
    }
}