using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Spawner : MonoBehaviour
{
    private DemonSpawner demonSpawner;
    public Vector3 position;

    private float timeBetweenSpawns;
    private float spawnTimer;

    private Queue<DemonType> demonsToSpawn = new Queue<DemonType>();

    private void Awake()
    {
        demonSpawner = FindObjectOfType<DemonSpawner>();
        position = transform.position;
    }

    private void Start()
    {
        timeBetweenSpawns = demonSpawner.timeBetweenSpawns;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;


        if(HelperFuntions.GreaterThanOrEqual(spawnTimer, timeBetweenSpawns))
        {
            if(demonsToSpawn.Count > 0)
            {
                SpawnDemon(demonsToSpawn.Dequeue(), demonSpawner);

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

    private void SpawnDemon(DemonType demon, DemonSpawner ds)
    {
        GameObject demonTemp = ds.demonPoolers[demon.Id].Spawn();

        DemonBase demonBase = demonTemp.GetComponent<DemonBase>();

        demonBase.OnSpawn(ds.player);

        demonTemp.transform.position = position;

        Debug.Log(position);

        ds.currentDemons++;
        ds.maxDemonsToSpawn--;
    }
}