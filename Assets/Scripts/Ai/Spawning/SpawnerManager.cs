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
public class SpawnerManager : MonoBehaviour
{
    [HideInInspector] public WaveManager WaveManager;
    [HideInInspector] public DemonSpawner DemonSpawner;
    

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

    [Header("Rituals")]
    public List<Ritual> Rituals = new List<Ritual>();
    [HideInInspector] public int RitualIndex = 0;
    [HideInInspector] public RitualSpawner currentRitual;

    private void Awake()
    {
        WaveManager = GetComponent<WaveManager>();
        DemonSpawner = GetComponent<DemonSpawner>();
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
//protected override void DoBuy(Interactor interactor)
//{
//    open = true;

//    foreach (Optional<Area> area in AreaConnections)
//    {
//        if (area.Enabled)
//        {
//            area.Value.discovered = true;

//            Spawners.GetDictionaryArea(DetectArea.CurrentArea, out Area currentArea);

//            foreach (Optional<AreaConnect> areasInConnections in area.Value.AdjacentAreas)
//            {
//                if (areasInConnections.Enabled)
//                {
//                    if (areasInConnections.Value.Area == currentArea)
//                    {
//                        areasInConnections.Value.Open = true;

//                        foreach (Optional<AreaConnect> AreasTouchingCurrentArea in areasInConnections.Value.Area.AdjacentAreas)
//                        {
//                            if (AreasTouchingCurrentArea.Value.Area == AreaConnection1.Value)
//                            {
//                                AreasTouchingCurrentArea.Value.Open = true;
//                                break;
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }

//    doAnimationStuff
//    transform.parent.gameObject.SetActive(false);
//}