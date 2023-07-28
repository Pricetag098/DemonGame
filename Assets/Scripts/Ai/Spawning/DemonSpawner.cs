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
    [SerializeField] ObjectPooler SprinterDemonPooler;

    Dictionary<Demon, ObjectPooler> demonPoolers = new Dictionary<Demon, ObjectPooler>();

    private void Awake()
    {
        demonPoolers.Add(baseDemon, baseDemonPooler);
        demonPoolers.Add(SprinterDemon, SprinterDemonPooler);
    }

    private void Start()
    {
        SpawnDemon(baseDemon, new Vector3(3, 5, 0));
        SpawnDemon(baseDemon, new Vector3(8, 5, 3));
        SpawnDemon(SprinterDemon, new Vector3(5, 5, 0));
    }

    private void Update()
    {
        
    }

    void SpawnDemon(Demon demon, Vector3 pos)
    {
        GameObject demonTemp = demonPoolers[demon].Spawn();

        DemonBase demonBase = demonTemp.GetComponent<DemonBase>();

        demonBase.OnSpawn(player);

        demonTemp.transform.position = pos;
    }

    int GetDemonSpawnChance(float min, float max)
    {
        // calculates each types spawn chance
        float chance = Random.Range(min, max);

        if(chance > 0)
        {
            float spawnFloat = 0;
            spawnFloat = demonsToSpawn * chance;
            return Mathf.FloorToInt(spawnFloat);
        }

        return 0;
    }

    List<string> ShuffleList(List<string> list)
    {
        return HelperFuntions.ShuffleList(list);
    }

    void OnWaveStart()
    {

    }

    void OnWaveEnd()
    {

    }

    
}
