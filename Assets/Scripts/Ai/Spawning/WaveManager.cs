using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int currentRound;

    [SerializeField] Wave wave;
    [SerializeField] Wave BaseWave;
    [SerializeField] Wave BossWave;

    [SerializeField] int BossWaveIncrement;

    [SerializeField] List<Wave> waves = new List<Wave>();
    private Wave[] WavesContainer = new Wave[101];

    private DemonSpawner spawner;

    [Header("Demons")]
    private int _base;
    private int _Summoner;
    private int _stalker;
    private int _choas;

    private void Awake()
    {
        spawner = GetComponent<DemonSpawner>();
    }

    private void Start()
    {
        SetWaves(waves);
        spawner.OnWaveStart += WaveStart;
        spawner.OnWaveEnd += WaveEnd;
    }

    public void NextWave()
    {

    }

    public void WaveStart()
    {
        wave = GetWave(currentRound);

        GetDemonSpawnCount();
    }
    public void WaveEnd()
    {
        currentRound++;
    }

    public void GetDemonSpawnCount()
    {
        //spawner.maxDemonsToSpawn = (int)spawner.demonsToSpawn.Evaluate(currentRound);
        //spawner.demonsToSpawnEachTick = (int)spawner.spawnsEachTick.Evaluate(currentRound);

        //_base = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.BasePercentage, spawner.maxDemonsToSpawn));
        //_Summoner = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.SummonerPercentage, spawner.maxDemonsToSpawn));
        //_stalker = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.StalkerPercentage, spawner.maxDemonsToSpawn));
        //_choas = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.ChoasPercentage, spawner.maxDemonsToSpawn));

        //int temp = spawner.maxDemonsToSpawn;

        List<DemonType> DemonsToSpawn = new List<DemonType>();
        List<DemonType> specialDemonTypes = new List<DemonType>();

        for (int i = 0; i < _base; i++)
        {
            DemonsToSpawn.Add(wave.Base);
        }

        //temp -= _base;

        for (int i = 0; i < _Summoner; i++)
        {
            specialDemonTypes.Add(wave.Summoner);
        }

        //temp -= _Summoner;

        for (int i = 0; i < _stalker; i++)
        {
            specialDemonTypes.Add(wave.Stalker);
        }

        //temp -= _stalker;

        for (int i = 0; i < _choas; i++)
        {
            specialDemonTypes.Add(wave.Summoner);
        }

        //temp -= _choas;
        //spawner.maxDemonsToSpawn -= temp;
        
        specialDemonTypes = HelperFuntions.ShuffleList(specialDemonTypes); // shuffles the special demon list

        int listSize = specialDemonTypes.Count;

#if UNITY_EDITOR
        HelperFuntions.ClearLog();
#endif

        //for (int i = 0; i < listSize; i++)
        //{
        //    // calculate at what position to add demon
        //    int index = Mathf.RoundToInt(HelperFuntions.GetRandomIndexBetweenMinMax(spawner.minMax.x, spawner.minMax.y, DemonsToSpawn.Count));

        //    Debug.Log("Index to add: " + index + " Max Size is: " + DemonsToSpawn.Count);

        //    DemonsToSpawn.Insert(index, specialDemonTypes[i]);
        //}

        //if (wave.BossWave == true) // add boss at 10% way through
        //{
        //    // calculate at what position to add demon
        //    int index = GetSpawnIndex(40, spawner.maxDemonsToSpawn);

        //    DemonsToSpawn.Insert(index, wave.Cultist);

        //    spawner.maxDemonsToSpawn++;
        //}

        spawner.DemonQueue = HelperFuntions.AddListToQueue(DemonsToSpawn);

        spawner.startRound = false;
        //spawner.canSpawn = true;
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
