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
    [HideInInspector] public DemonSpawner _DemonSpawner;
    [HideInInspector] public RitualManager RitualManager;
    [HideInInspector] public BlessingManager BlessingManager;
    [HideInInspector] public RoundDisplay roundDisplay;

    [Header("Player")]
    public Transform player;

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
    [Range(1, 10000)] public int currentRound;
    public int maxDemonsToSpawn;
    public int currentDemons;
    public bool EndOfRound;
    public bool StartOfRound;
    public bool RunDefaultSpawning;

    [Header("Timers")]
    private Timer spawnTimer;
    private Timer endRoundTimer;

    [Header("Spawn Particle")]
    [HideInInspector] public ObjectPooler ParticleSpawner;

    private void Awake()
    {
        WaveManager = GetComponent<WaveManager>();
        _DemonSpawner = GetComponent<DemonSpawner>();
        RitualManager = GetComponent<RitualManager>();
        BlessingManager = GetComponent<BlessingManager>();
        ParticleSpawner = GetComponent<ObjectPooler>();
        roundDisplay = FindObjectOfType<RoundDisplay>();

        spawnTimer = new Timer(timeBetweenSpawns);
        endRoundTimer = new Timer(timeBetweenRounds);
    }
    private void Start()
    {
        WaveStart();
    }

    private void Update()
    {
        if(RunDefaultSpawning == true)
        {
            Bools();

            if (EndOfRound == true)
            {
                WaveEnd();
                EndOfRound = false;
                StartOfRound = true;
            }

            if (StartOfRound == true)
            {
                if (endRoundTimer.TimeGreaterThan)
                {
                    WaveStart();
                    StartOfRound = false;
                }
            }

            if (spawnTimer.TimeGreaterThan && canSpawn == true)
            {
                if (HelperFuntions.IntGreaterThanOrEqual(maxDemonsAtOnce, currentDemons))
                {
                    if (_DemonSpawner.DemonCount == 0) // if no demons to spawn return
                    {
                        return;
                    }

                    int toSpawn = maxDemonsAtOnce - currentDemons;

                    if (toSpawn >= demonsToSpawnEachTick) { toSpawn = demonsToSpawnEachTick; }

                    if (maxDemonsToSpawn < toSpawn) { toSpawn = maxDemonsToSpawn; }

                    if (toSpawn > 0)
                    {
                        _DemonSpawner.ResetSpawners();

                        for (int i = 0; i < toSpawn; i++)
                        {
                            if(_DemonSpawner.SpawnDemon(this))
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

    private void LateUpdate()
    {
        _DemonSpawner.CallDemonUpdatePosition();
    }

    public void GetBlessingChance(Transform pos, bool spawnDrop = false)
    {
        BlessingManager.GetBlessingChance(pos, currentRound, spawnDrop);
    }

    public void UpdateSpawners(Areas Id, Areas CurrentArea)
    {
        _DemonSpawner.ActiveSpawners(Id, CurrentArea);
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

        _DemonSpawner.DemonQueue = WaveManager.GetDemonToSpawn(maxDemonsToSpawn);
    }

    void WaveEnd()
    {
        roundDisplay.ColourFlash();

        _DemonSpawner.DemonQueue.Clear();
        currentRound++;
    }

    public void DemonKilled()
    {
        currentDemons--;
    }

    public void DespawnAllActiveDemons()
    {
        _DemonSpawner.DespawnAllActiveDemons();
    }

    public void KillAllActiveDemons()
    {
        _DemonSpawner.KillAllActiveDemons();
    }

    public static List<GameObject> AllActiveDemons()
    {
        List<GameObject> list = new List<GameObject>();

        foreach(var demon in DemonSpawner.ActiveDemons)
        {
            list.Add(demon.gameObject);
        }

        return list;
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