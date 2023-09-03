using System.Collections.Generic;
using UnityEngine;

public class RitualSpawner : MonoBehaviour
{
    public Ritual ritual;

    [Header("Display Variables")]
    public bool RitualActive;
    public bool ritualComplete;
    public int currentDemons;
    public int demonsLeft;
    public int demonsToSpawn;

    [Header("Spawn Points")]
    [SerializeField] Transform SpawnPoint;
    private List<Spawner> spawnPoints = new List<Spawner>();

    [Header("Demons")]
    public Queue<DemonType> DemonQueue = new Queue<DemonType>();

    private SpawnerManager manager;
    private DemonSpawner demonSpawner;

    private float timer;

    private void Awake()
    {
        manager = FindObjectOfType<SpawnerManager>();
        spawnPoints = HelperFuntions.GetAllChildrenSpawnersFromParent(SpawnPoint);
    }

    private void Start()
    {
        demonSpawner = manager.DemonSpawner;
    }

    private void Update()
    {
        if(RitualActive)
        {
            Spawning(demonSpawner, manager);
        }
    }

    public void InitaliseRitual()
    {
        if(RitualActive == false && ritualComplete == false)
        {
            RitualActive = true;

            SetDemonQueue(ritual.ritualWave);

            demonsLeft = ritual.demonsToSpawn;
            demonsToSpawn = ritual.demonsToSpawn;

            Debug.Log("Demon Queue count: " + DemonQueue.Count);
        }
    }

    public void Spawning(DemonSpawner spawner, SpawnerManager sm)
    {
        timer += Time.deltaTime;

        if (HelperFuntions.TimerGreaterThan(timer, ritual.TimeBetweenSpawns) && RitualActive == true)
        {
            if (HelperFuntions.IntGreaterThanOrEqual(ritual.MaxDemonsAtOnce, currentDemons))
            {
                timer = 0;

                if (demonsLeft <= 0 && currentDemons <= 0 && demonsToSpawn <= 0)
                {   ritualComplete = true; RitualActive = false;
                    sm.RunDefaultSpawning = true; 
                    sm.currentRitual = null; return; 
                }

                if (DemonQueue.Count <= 0) { return; }

                int toSpawn = ritual.MaxDemonsAtOnce - currentDemons;

                if (toSpawn >= ritual.SpawnsPerTick) { toSpawn = ritual.SpawnsPerTick; }

                if (demonsToSpawn < toSpawn) { toSpawn = demonsToSpawn; }

                spawnPoints = HelperFuntions.ShuffleList(spawnPoints);

                foreach (var spawnPoint in spawnPoints) { spawnPoint.Visited = false; }

                if (toSpawn > 0)
                {
                    for (int i = 0; i < toSpawn; i++)
                    {
                        if (spawner.SpawnDemonRitual(spawnPoints, this, sm))
                        {
                            currentDemons++;
                            demonsToSpawn--;
                        }
                    }
                }
            }
        }
    }

    public void OnComplete()
    {

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

    public int DemonCount
    {
        get { return DemonQueue.Count; }
    }

}
