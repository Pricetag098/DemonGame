using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlessingManager : MonoBehaviour
{
    [SerializeField] private Vector2Int MinMax;
    private int Counter;
    private int SpawnNum;
    private BlessingSpawner blessingSpawner;

    private void Awake()
    {
        blessingSpawner = FindObjectOfType<BlessingSpawner>();
        SpawnNum = Random.Range(MinMax.x, MinMax.y);
    }

    public void SpawnBlessingOfType(Transform pos, BlessingType type = BlessingType.Null)
    {
        blessingSpawner.SpawnBlessing(pos, type);
    }

    public void GetBlessingChance(Transform pos)
    {   
        if (Counter != SpawnNum)
        {
            Counter++;
            return;
        }

        SpawnNum = Random.Range(MinMax.x, MinMax.y);
        Counter = 0;
        blessingSpawner.SpawnBlessing(pos);
    }
}
