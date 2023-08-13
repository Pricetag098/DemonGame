using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField] bool discovered;
    private DemonSpawner demonSpawner;
    [SerializeField] List<Spawner> baseSpawns;
    [SerializeField] List<Spawner> specialSpawns;

    private void Awake()
    {
        demonSpawner = FindObjectOfType<DemonSpawner>();
    }

    public void SpawnLocations()
    {
        if(discovered == false)
        {
            demonSpawner.baseSpawners = HelperFuntions.AddToList(demonSpawner.baseSpawners, baseSpawns);
            demonSpawner.specialSpawners = HelperFuntions.AddToList(demonSpawner.specialSpawners, specialSpawns);

            baseSpawns = null;
            specialSpawns = null;

            discovered = true;
        }
    }
}
