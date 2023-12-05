using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonInfo;

/// <summary>
/// percentage based spawning
/// </summary>

[CreateAssetMenu(fileName = "Wave", menuName = "Wave/Create Wave", order = 0)]
public class Wave : ScriptableObject
{
    [Header("Wave")]
    public SpawnType spawnType;
    public int Round;

    [Header("Demons")]
    public DemonType Base;
    public float BasePercentage;
    public float WalkerPercent;
    public float JoggerPercent;
    public float RunnerPercent;

    public DemonType LittleGuy;
    public float LittleGuyPercentage;

    public DemonType Choas;
    public int ChoasAmount;
    public float WaveInsertPosition;

    [Header("Extra Chaos Chance")]
    [Range(0,100)] public float ChanceToSpawnChaos;
    [Range(0,10)] public int MaxChoasToSpawn;

    public static Queue<DemonType> GetWave(int DemonsToAdd, Wave wave)
    {
        List<DemonType> demons = new List<DemonType>();

        int _base = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.BasePercentage, DemonsToAdd));
        int _littleGuy = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.LittleGuyPercentage, DemonsToAdd));

        int counter = DemonsToAdd;

        AddTypeToList(wave.Base, _base, demons);

        counter -= _base;

        AddTypeToList(wave.LittleGuy, _littleGuy, demons);

        counter -= _littleGuy;

        AddTypeToList(wave.Base, counter, demons);

        demons = HelperFuntions.ShuffleList(demons);

        return HelperFuntions.AddListToQueue(demons);
    }

    public static void AddTypeToList<T>(T type, int amount, List<T> list)
    {
        for (int i = 0; i < amount; i++)
        {
            list.Add(type);
        }
    }
}