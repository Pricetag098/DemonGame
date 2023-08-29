using DemonInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ritual : MonoBehaviour
{
    [Header("Wave")]
    public Wave ritualWave;

    [SerializeField] private int demonsToSpawn;
    [SerializeField] private int MaxDemonsAtOnce;
    [SerializeField] private int SpawnsPerTick;
    [SerializeField] private int TimeBetweenSpawns;

    [Header("Time of Ritual")]
    [SerializeField] float TotalTimeOfRitual;

    [Header("Display Variables")]
    public bool RitualActive;
    public bool ritualComplete;
    public int currentDemons;
    [SerializeField] private int demonsLeft;

    [Header("Spawn Points")]
    public List<Spawner> spawnPoints = new List<Spawner>();

    [Header("Demons")]
    public Queue<DemonType> DemonQueue = new Queue<DemonType>();

    private float timer;
    SpawnerManager sm;

    private void Awake()
    {
        // set the varibles from the wave for the ritual
        // set demons to queue
    }

    public void InitaliseRitual()
    {
        if(RitualActive == false && ritualComplete == false)
        {
            RitualActive = true;

            SetDemonQueue(ritualWave);

            demonsLeft = demonsToSpawn;
        }
        
    }

    public void Spawning(DemonSpawner spawner, SpawnerManager sm)
    {
        timer += Time.deltaTime;

        if (HelperFuntions.TimerGreaterThan(timer, TimeBetweenSpawns) && RitualActive == true)
        {
            if (HelperFuntions.IntGreaterThanOrEqual(MaxDemonsAtOnce, currentDemons))
            {
                timer = 0;

                if(demonsLeft <= 0 && currentDemons <= 0) { ritualComplete = true; sm.RitualSpawning = false; sm.RunDefaultSpawning = true;  sm.currentRitual = null; return; }

                if(DemonQueue.Count <= 0) { return; }

                int toSpawn = MaxDemonsAtOnce - currentDemons;

                if (toSpawn <= SpawnsPerTick) { }
                else { toSpawn = SpawnsPerTick; }

                if (demonsToSpawn < toSpawn) { toSpawn = demonsToSpawn; }

                spawnPoints = HelperFuntions.ShuffleList(spawnPoints);

                for (int i = 0; i < toSpawn; i++)
                {
                    if (spawner.SpawnDemon(spawnPoints, this, sm))
                    {
                        currentDemons++;
                        demonsLeft--;
                    }
                }
            }
        }
    }

    public void SetWave(Wave wave)
    {
        ritualWave = wave;
    }

    void SetDemonQueue(Wave wave)
    {
        List<DemonType> demons = new List<DemonType>();

        int _base = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.BasePercentage, demonsToSpawn));
        int _Summoner = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.SummonerPercentage, demonsToSpawn));
        int _stalker = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.StalkerPercentage, demonsToSpawn));
        int _choas = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.ChoasPercentage, demonsToSpawn));

        int counter = demonsToSpawn;

        AddTypeToList(wave.Base, _base, demons);

        counter -= _base;

        AddTypeToList(wave.Summoner, _Summoner, demons);

        counter -= _Summoner;

        AddTypeToList(wave.Stalker, _stalker, demons);

        counter -= _stalker;

        AddTypeToList(wave.Choas, _choas, demons);

        counter -= _choas;

        AddTypeToList(wave.Base, counter, demons);

        demons = HelperFuntions.ShuffleList(demons);

        DemonQueue = HelperFuntions.AddListToQueue(demons);
    }

    void AddTypeToList(DemonType type, int amount, List<DemonType> list)
    {
        for (int i = 0; i < amount; i++)
        {
            list.Add(type);
        }
    }
}
