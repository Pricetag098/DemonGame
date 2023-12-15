using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BlessingManager : MonoBehaviour
{
    private int Counter;
    private int SpawnNum;

    public int percentageChance;

    private BlessingSpawner blessingSpawner;

    [SerializeField] private AnimationCurve minRound;
    [SerializeField] private AnimationCurve maxRound;

    private float min;
    private float max;

    [SerializeField] Transform spawnpoint;

    private void Awake()
    {
        blessingSpawner = FindObjectOfType<BlessingSpawner>();
        MinAndMax(1);
    }

    [ContextMenu("test")]
    public void TestSpawn()
    {
        SpawnBlessingOfType(spawnpoint);
    }

    public void SpawnBlessingOfType(Transform pos, BlessingType type = BlessingType.Null)
    {
        blessingSpawner.SpawnBlessing(pos, type);
    }

    public void MinAndMax(float currentRound)
    {
        min = minRound.Evaluate(currentRound);
        max = maxRound.Evaluate(currentRound);
        SpawnNum = (int)UnityEngine.Random.Range(min, max);
    }

    public void GetBlessingChance(Transform pos, int currentRound, bool spawnDrop = false)
    {   
        if(Counter >= SpawnNum)
        {
            if(spawnDrop == true)
            {
                MinAndMax(currentRound);
                Counter = 0;
                blessingSpawner.SpawnBlessing(pos);
                return;
            }
        }

        Counter++;
    }

    public void PercentChance(Transform pos)
    {
        int rand = UnityEngine.Random.Range(1, 101);

        if(rand <= percentageChance)
        {
            blessingSpawner.SpawnBlessing(pos);
        }
    }
}
