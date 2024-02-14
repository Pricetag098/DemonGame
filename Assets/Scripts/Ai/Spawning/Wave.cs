using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonInfo;
//using Palmmedia.ReportGenerator.Core.Logging;
using static Unity.Collections.Unicode;

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

    public DemonType Choas;
    public List<float> WavePositions = new List<float>();

    public static Queue<DemonType> GetWave(int DemonsToAdd, Wave wave)
    {
        List<DemonType> demons = new List<DemonType>();

        int _base = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.BasePercentage, DemonsToAdd));
        int jogger = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.JoggerPercent, _base));
        int runner = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.RunnerPercent, _base));

        jogger += runner;

        int counter = DemonsToAdd;

        for (int i = 0; i < _base; i++)
        {
            DemonType tempBase = new DemonType();
            tempBase.SpawnType = wave.Base.SpawnType;
            tempBase.Id = wave.Base.Id;
            tempBase.SpawnerType = wave.Base.SpawnerType;

            if (i < runner) { tempBase.SpeedType = SpeedType.Runner; }
            else if (i < jogger) { tempBase.SpeedType = SpeedType.Jogger; }
            else { tempBase.SpeedType = SpeedType.Walker; }

            demons.Add(tempBase);
        }

        counter -= _base;

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