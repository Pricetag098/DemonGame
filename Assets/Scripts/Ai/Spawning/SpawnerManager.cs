using DemonInfo;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(WaveManager))]
[RequireComponent(typeof(DemonSpawner))]
[RequireComponent(typeof(DemonPoolers))]
[RequireComponent(typeof(Spawners))]
[RequireComponent(typeof(RitualManager))]
public class SpawnerManager : MonoBehaviour
{
    [HideInInspector] public WaveManager WaveManager;
    [HideInInspector] public DemonSpawner DemonSpawner;
    [HideInInspector] public RitualManager RitualManager;
    

    [Header("Player")]
    public Transform player;
    [SerializeField] NavMeshAgent playerAgent;

    [Header("Spawning Stats")]
    public bool canSpawn;
    [SerializeField] int maxDemonsAtOnce;
    public float timeBetweenSpawns;
    [SerializeField] float timeBetweenRounds;
    public int demonsToSpawnEachTick;
    public Vector2Int minMax;

    [Header("Animation Curves")]
    public AnimationCurve demonsToSpawn;
    public AnimationCurve spawnsEachTick;

    [Header("Display Stats")]
    [Range(0, 10000)] public int currentRound;
    public int maxDemonsToSpawn;
    public int currentDemons;
    public bool EndOfRound;
    public bool StartOfRound;
    public bool RunDefaultSpawning;

    [Header("Timers")]
    private float spawnTimer;
    private float endRoundTimer;

    private void Awake()
    {
        WaveManager = GetComponent<WaveManager>();
        DemonSpawner = GetComponent<DemonSpawner>();
        RitualManager = GetComponent<RitualManager>();
    }
    private void Start()
    {
        RunDefaultSpawning = true;

        WaveStart();
    }

    private void Update()
    {
        if(RunDefaultSpawning == true)
        {
            Timers();
            Bools();

            if (EndOfRound == true)
            {
                WaveEnd();
                EndOfRound = false;
                StartOfRound = true;
            }

            if (StartOfRound == true)
            {
                endRoundTimer += Time.deltaTime;
                if (HelperFuntions.TimerGreaterThan(endRoundTimer, timeBetweenRounds))
                {
                    WaveStart();
                    endRoundTimer = 0f;
                    StartOfRound = false;
                }
            }

            

            if (HelperFuntions.TimerGreaterThan(spawnTimer, timeBetweenSpawns) && canSpawn == true)
            {
                if (HelperFuntions.IntGreaterThanOrEqual(maxDemonsAtOnce, currentDemons))
                {
                    spawnTimer = 0;

                    if (DemonSpawner.DemonCount <= 0) // if no demons to spawn return
                    {
                        return;
                    }

                    int toSpawn = maxDemonsAtOnce - currentDemons;

                    if (toSpawn >= demonsToSpawnEachTick) { toSpawn = demonsToSpawnEachTick; }
                    else {  }

                    if (maxDemonsToSpawn < toSpawn) { toSpawn = maxDemonsToSpawn; }

                    if (toSpawn > 0)
                    {
                        DemonSpawner.ResetSpawners();

                        for (int i = 0; i < toSpawn; i++)
                        {
                            if(DemonSpawner.SpawnDemon(this))
                            {
                                currentDemons++;
                                maxDemonsToSpawn--;
                            }
                        }
                    }
                }
            }
        }
    }

    public void UpdateSpawners(Areas Id, Areas CurrentArea)
    {
        DemonSpawner.ActiveSpawners(Id, CurrentArea);
    }

    public Ritual GetCurrentRitual()
    {
        return RitualManager.GetCurrentRitual();
    }

    public void IncrementRitualIndex()
    {
        RitualManager.IncrementIndex();
    }
    public void SetCurrentRitual(RitualSpawner rs)
    {
        RitualManager.SetCurrentRitual(rs);
    }

    public void FinalRitual()
    {
        RitualManager.FinalRitual();
    }
    public void CurrentRitualOnDemonDeath()
    {
        RitualManager.CurrentRitualOnDemonDeath();
    }

    public void TpPlayerOnStart()
    {
        RitualManager.TpPlayerOnStart();
    }

    public void TpPlayerOnEnd()
    {
        RitualManager.TpPlayerOnEnd();
    }

    public void AddDemonBackToRitual(DemonType type)
    {
        RitualManager.AddDemonBackToRitual(type);
    }

    void WaveStart()
    {
        WaveManager.NextWave(currentRound);

        maxDemonsToSpawn = MaxDemonsToSpawn;
        demonsToSpawnEachTick = DemonSpawnsEachTick;

        DemonSpawner.DemonQueue = WaveManager.GetDemonToSpawn(maxDemonsToSpawn);

        currentRound++;
    }

    void WaveEnd()
    {
        DemonSpawner.DemonQueue.Clear();
    }

    public void DemonKilled()
    {
        currentDemons--;
    }

    public void DespawnAllActiveDemons()
    {
        DemonSpawner.DespawnAllActiveDemons();
    }

    void Timers()
    {
        spawnTimer += Time.deltaTime;
    }

    void Bools()
    {
        if (maxDemonsToSpawn <= 0 && currentDemons <= 0 && StartOfRound == false) EndOfRound = true;
    }

    public int MaxDemonsToSpawn
    {
        get { return HelperFuntions.EvaluateAnimationCuveInt(demonsToSpawn, currentRound); }
    }

    public int DemonSpawnsEachTick
    {
        get { return HelperFuntions.EvaluateAnimationCuveInt(spawnsEachTick, currentRound); }
    }
}