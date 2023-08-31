using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField] bool discovered;
    private Spawners spawner;
    [SerializeField] List<Spawner> baseSpawns;
    [SerializeField] List<Spawner> specialSpawns;

    private void Awake()
    {
        spawner = FindObjectOfType<Spawners>();
    }

    public void SpawnLocations()
    {
        if(discovered == false)
        {
            spawner.baseSpawners = HelperFuntions.AddToList(spawner.baseSpawners, baseSpawns);
            spawner.specialSpawners = HelperFuntions.AddToList(spawner.specialSpawners, specialSpawns);

            baseSpawns = null;
            specialSpawns = null;

            discovered = true;
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.magenta;
    //    foreach (var spawner in baseSpawns)
    //    {
    //        Gizmos.DrawCube(spawner.position, new Vector3(2, 2, 2));
    //    }

    //    Gizmos.color = Color.red;
    //    foreach(var spawner in specialSpawns)
    //    {
    //        Gizmos.DrawCube(spawner.position, new Vector3(2, 2, 2));
    //    }
    //}
}
