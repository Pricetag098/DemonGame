using DemonInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualSpawner : MonoBehaviour
{
    public Ritual ritual;

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

    public void InitaliseRitual()
    {
        if(RitualActive == false && ritualComplete == false)
        {
            RitualActive = true;

            SetDemonQueue(ritual.ritualWave);

            demonsLeft = ritual.demonsToSpawn;
        }
        
    }

    public void Spawning(DemonSpawner spawner, SpawnerManager sm)
    {
        if(RitualActive)
        {
            timer += Time.deltaTime;

            if (HelperFuntions.TimerGreaterThan(timer, ritual.TimeBetweenSpawns) && RitualActive == true)
            {
                if (HelperFuntions.IntGreaterThanOrEqual(ritual.MaxDemonsAtOnce, currentDemons))
                {
                    timer = 0;

                    if (demonsLeft <= 0 && currentDemons <= 0)
                    {   ritualComplete = true; RitualActive = false; 
                        sm.RitualSpawning = false; sm.RunDefaultSpawning = true; 
                        sm.currentRitual = null; return; 
                    }

                    if (DemonQueue.Count <= 0) { return; }

                    int toSpawn = ritual.MaxDemonsAtOnce - currentDemons;

                    if (toSpawn <= ritual.SpawnsPerTick) { }
                    else { toSpawn = ritual.SpawnsPerTick; }

                    if (ritual.demonsToSpawn < toSpawn) { toSpawn = ritual.demonsToSpawn; }

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
    }

    void SetDemonQueue(Wave wave)
    {
        List<DemonType> demons = new List<DemonType>();

        int _base = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.BasePercentage, ritual.demonsToSpawn));
        int _Summoner = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.SummonerPercentage, ritual.demonsToSpawn));
        int _stalker = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.StalkerPercentage, ritual.demonsToSpawn));
        int _choas = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.ChoasPercentage, ritual.demonsToSpawn));

        int counter = ritual.demonsToSpawn;

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
