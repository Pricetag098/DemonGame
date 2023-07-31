using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSpawner : MonoBehaviour
{
    [SerializeField] Transform player;

    [Header("Demons")]
    [SerializeField] Demon baseDemon;
    [SerializeField] Demon SprinterDemon;

    [Header("Wave")]
    [SerializeField] int wave;

    [Header("Spawning Stats")]
    [SerializeField] int demonsToSpawn;
    [SerializeField] int maxSpawningDistance;

    [Header("Object Poolers")]
    [SerializeField] ObjectPooler baseDemonPooler;
    [SerializeField] ObjectPooler sprinterDemonPooler;

    Dictionary<DemonId, ObjectPooler> demonPoolers = new Dictionary<DemonId, ObjectPooler>();

    Queue<DemonId> DemonQueue = new Queue<DemonId>();

    private void Awake()
    {
        demonPoolers.Add(DemonId.Walker, baseDemonPooler);
        demonPoolers.Add(DemonId.Sprinter, sprinterDemonPooler);
        //DemonQueue.Enqueue(DemonId.Sprinter);
    }

    private void Start()
    {
        //SpawnDemon(DemonQueue.Dequeue(), new Vector3(15, 5, 0));
    }

    private void Update()
    {
        
    }

    void SpawnDemon(DemonId demon, Vector3 pos) // spawns demon at location
    {
        GameObject demonTemp = demonPoolers[demon].Spawn();

        DemonBase demonBase = demonTemp.GetComponent<DemonBase>();

        demonBase.OnSpawn(player);

        demonTemp.transform.position = pos;
    }

    int GetDemonSpawnChance(float min, float max) // calculates each types spawn chance
    {
        float chance = Random.Range(min, max);

        if(chance > 0)
        {
            float spawnFloat = 0;
            spawnFloat = demonsToSpawn * chance;
            return Mathf.FloorToInt(spawnFloat);
        }

        return 0;
    }

    void AddListToQueue(Queue q, List<DemonId> list)
    {
        foreach(DemonId item in list) 
        {
            q.Enqueue(item);
        }
    }

    List<T> ShuffleList<T>(List<T> list)
    {
        return HelperFuntions.ShuffleList(list);
    }

    

    void OnWaveStart(Wave currentwave)
    {

    }

    void OnWaveEnd()
    {

    }
}
