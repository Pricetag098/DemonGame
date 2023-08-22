using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private SpawnerManager Manager;

    [SerializeField] Wave wave;
    [SerializeField] Wave BaseWave;
    [SerializeField] Wave BossWave;

    [SerializeField] int BossWaveIncrement;

    [SerializeField] List<Wave> waves = new List<Wave>();
    private Wave[] WavesContainer = new Wave[101];

    [Header("Demons")]
    private int _base;
    private int _Summoner;
    private int _stalker;
    private int _choas;

    private void Awake()
    {
        Manager = GetComponent<SpawnerManager>();
        SetWaves(waves);
    }

    public void NextWave(int currentRound)
    {
        wave = GetWave(currentRound);
    }

    public Queue<DemonType> GetDemonToSpawn(int MaxToSpawn)
    {
        _base = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.BasePercentage, MaxToSpawn));
        _Summoner = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.SummonerPercentage, MaxToSpawn));
        _stalker = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.StalkerPercentage, MaxToSpawn));
        _choas = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.ChoasPercentage, MaxToSpawn));

        List<DemonType> DemonsToSpawn = new List<DemonType>();
        List<DemonType> specialDemonTypes = new List<DemonType>();

        for (int i = 0; i < _base; i++)
        {
            DemonsToSpawn.Add(wave.Base);
        }

        MaxToSpawn -= _base;

        for (int i = 0; i < _Summoner; i++)
        {
            specialDemonTypes.Add(wave.Summoner);
        }

        MaxToSpawn -= _Summoner;

        for (int i = 0; i < _stalker; i++)
        {
            specialDemonTypes.Add(wave.Stalker);
        }

        MaxToSpawn -= _stalker;

        for (int i = 0; i < _choas; i++)
        {
            specialDemonTypes.Add(wave.Summoner);
        }

        MaxToSpawn -= _choas;
        Manager.maxDemonsToSpawn -= MaxToSpawn;
        
        specialDemonTypes = HelperFuntions.ShuffleList(specialDemonTypes); // shuffles the special demon list

        int listSize = specialDemonTypes.Count;

#if UNITY_EDITOR
        HelperFuntions.ClearLog();
#endif

        for (int i = 0; i < listSize; i++)
        {
            // calculate at what position to add demon
            int index = Mathf.RoundToInt(HelperFuntions.GetRandomIndexBetweenMinMax(Manager.minMax.x, Manager.minMax.y, DemonsToSpawn.Count));

            //Debug.Log("Index to add: " + index + " Max Size is: " + DemonsToSpawn.Count);

            DemonsToSpawn.Insert(index, specialDemonTypes[i]);
        }

        if (wave.BossWave == true) // add boss at 10% way through
        {
            // calculate at what position to add demon
            int index = GetSpawnIndex(40, Manager.maxDemonsToSpawn);

            DemonsToSpawn.Insert(index, wave.Cultist);

            Manager.maxDemonsToSpawn++;
        }

        Manager.StartOfRound = false;
        Manager.canSpawn = true;

        return HelperFuntions.AddListToQueue(DemonsToSpawn);
    }
    private void SetWaves(List<Wave> list)
    {
        foreach (Wave w in list)
        {
            if (w.Round != 0)
            {
                WavesContainer[w.Round] = w;
            }
        }

        int count = WavesContainer.Length;

        for (int i = 0; i < count; i++)
        {
            if (i % BossWaveIncrement == 0 && i != 5)
            {
                WavesContainer[i] = BossWave;
            }
        }

        for (int i = 0; i < count; i++) // set all left over rounds as base rounds
        {
            if (WavesContainer[i] == null)
            {
                WavesContainer[i] = BaseWave;
            }
        }
    }
    public int GetSpawnIndex(float percentage, int total)
    {
        return Mathf.RoundToInt(percentage / 100 * total);
    }
    public Wave GetWave(int currentRound)
    {
        if (currentRound >= WavesContainer.Length) return wave = BaseWave;
        return wave = WavesContainer[currentRound];
    }
}
