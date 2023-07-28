using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSpawner : MonoBehaviour
{
    [Header("Wave")]
    [SerializeField] int wave;

    [Header("Demons")]
    [SerializeField] Demon baseDemon;
    [SerializeField] Demon SprinterDemon;

    [Header("Object Poolers")]
    [SerializeField] ObjectPooler baseDemonPooler;
    [SerializeField] ObjectPooler SprinterDemonPooler;

    [Header("Spawning Stats")]
    [SerializeField] int maxSpawningDistance;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    void SpawnDemon(string id)
    {
        // calls into the pooler
    }

    void GetDemonSpawnChance(float min, float max)
    {
        // calculates each types spawn chance
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
