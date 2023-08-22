using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Spawner : MonoBehaviour
{
    private DemonSpawner demonSpawner;
    private SpawnerManager Manager;

    public Vector3 position;

    private float timeBetweenSpawns;
    private float spawnTimer;

    private Queue<DemonType> demonsToSpawn = new Queue<DemonType>();

    private void Awake()
    {
        demonSpawner = FindObjectOfType<DemonSpawner>();
        Manager = FindObjectOfType<SpawnerManager>();
        position = transform.position;
    }
    private void Start()
    {
        timeBetweenSpawns = Manager.timeBetweenSpawns;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if(HelperFuntions.GreaterThanOrEqual(spawnTimer, timeBetweenSpawns))
        {
            if(demonsToSpawn.Count > 0)
            {
                SpawnDemon(demonsToSpawn.Dequeue());

                spawnTimer = 0;
            }
        }
    }

    public bool RequestSpawn(DemonType demon)
    {
        if(demonsToSpawn.Count < 10)
        {
            demonsToSpawn.Enqueue(demon);
            return true;
        }

        demonSpawner.DemonQueue.Enqueue(demon);

        return false;
    }

    private void SpawnDemon(DemonType demon)
    {
        GameObject demonTemp = Manager.DemonPoolers.demonPoolers[demon.Id].Spawn();

        DemonBase demonBase = demonTemp.GetComponent<DemonBase>();

        demonBase.OnSpawn(Manager.player);

        demonTemp.transform.position = position;

        Manager.currentDemons++;
        Manager.maxDemonsToSpawn--;
    }
}