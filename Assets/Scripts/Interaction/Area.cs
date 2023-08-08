using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField] bool discovered;
    [SerializeField] DemonSpawner demonSpawner;
    [SerializeField] List<Transform> baseSpawns;
    [SerializeField] List<Transform> specialSpawns;
    
    public void SpawnLocations()
    {
        if(discovered == false)
        {
            demonSpawner.baseSpawners = HelperFuntions.AddToList(demonSpawner.baseSpawners, baseSpawns);
            demonSpawner.specialSpawners = HelperFuntions.AddToList(demonSpawner.specialSpawners, specialSpawns);

            baseSpawns = null;
            specialSpawns = null;

            discovered = true;

            Debug.Log("wall buys work");
        }
    }
}
