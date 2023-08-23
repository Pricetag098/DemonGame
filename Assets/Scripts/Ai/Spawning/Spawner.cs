using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Spawner : MonoBehaviour
{
    public Vector3 position;

    private DemonSpawner demonSpawner;
    private Transform player;
    private float timeBetweenSpawns;
    private float spawnTimer;

    public Queue<DemonType> demonsToSpawn = new Queue<DemonType>();

    private void Awake()
    {
        demonSpawner = FindObjectOfType<DemonSpawner>();
        SpawnerManager sm = FindObjectOfType<SpawnerManager>();
        player = sm.player;
        timeBetweenSpawns = sm.timeBetweenSpawns;

        position = transform.position;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if(HelperFuntions.GreaterThanOrEqual(spawnTimer, timeBetweenSpawns))
        {
            if(demonsToSpawn.Count > 0)
            {
                SpawnDemon(demonsToSpawn.Dequeue(), demonSpawner.demonPool, player);

                spawnTimer = 0;
            }
        }
    }

    public bool RequestSpawn(DemonType demon, DemonSpawner spawner)
    {
        if(demonsToSpawn.Count < 10)
        {
            demonsToSpawn.Enqueue(demon); // spawns demon
            return true;
        }

        spawner.DemonQueue.Enqueue(demon); // returns demon to queue

        return false;
    }

    private void SpawnDemon(DemonType demon, DemonPoolers pool, Transform target)
    {
        GameObject demonTemp = pool.demonPoolers[demon.Id].Spawn();

        DemonBase demonBase = demonTemp.GetComponent<DemonBase>();

        demonBase.OnSpawn(target);

        demonTemp.transform.position = position;
    }
}