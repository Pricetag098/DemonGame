using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBuy : ShopInteractable
{
    public bool open;
    [SerializeField] Vector3 openOffset;
    [SerializeField] DemonSpawner demonSpawner;
    [SerializeField] List<Transform> baseSpawns;
    [SerializeField] List<Transform> specialSpawns;

    protected override bool CanBuy()
    {
        return !open;
    }
    protected override void DoBuy()
    {
        open = true;
        //doAnimationStuff
        transform.parent.position += openOffset;

        SpawnLocations(baseSpawns, specialSpawns);
    }

    public void SpawnLocations(List<Transform> baseList, List<Transform> specialList)
    {
        demonSpawner.baseSpawners = HelperFuntions.AddToList(demonSpawner.baseSpawners, baseList);
        demonSpawner.specialSpawners = HelperFuntions.AddToList(demonSpawner.specialSpawners, specialList);

        baseList.Clear();
        specialList.Clear();

        Debug.Log("wall buys work");
    }
}
