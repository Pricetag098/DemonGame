using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonCum;
using System.Reflection;
using Unity.Jobs;
using UnityEngine.AI;

public class DemonSpawner : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] NavMeshAgent playerAgent;

    [Header("Wave")]
    public int currentRound;
    [SerializeField] Wave wave;
    [SerializeField] Wave BaseWave;
    [SerializeField] Wave BossWave;
    [SerializeField] List<Wave> waves = new List<Wave>();

    [SerializeField] int BossWaveIncrement;

    private Wave[] WavesContainer = new Wave[101];

    [Header("Display Stats")]
    [SerializeField] int maxDemonsToSpawn;
    [SerializeField] int currentDemons;

    [Header("Spawning Stats")]
    [SerializeField] bool canSpawn;
    [SerializeField] int maxDemonsAtOnce;
    [SerializeField] float maxPathingDistance;
    [SerializeField] float maxSpawningDistance;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] float timeBetweenRounds;
    [SerializeField] int demonsToSpawnEachTick;
    [SerializeField] Vector2Int minMax;

    [SerializeField] private bool endRound;
    [SerializeField] private bool startRound;

    [Header("Animation Curves")]
    [SerializeField] AnimationCurve demonsToSpawn;
    [SerializeField] AnimationCurve spawnsEachTick;

    [Header("Spawn Location")]
    [SerializeField] Transform baseSpawner;
    private List<Transform> baseActiveSpawners = new List<Transform>();
    [HideInInspector] public List<Transform> baseSpawners = new List<Transform>();

    [SerializeField] Transform SpecialSpawner;
    [HideInInspector] List<Transform> specialActiveSpawners = new List<Transform>();
    [HideInInspector] public List<Transform> specialSpawners = new List<Transform>();

    [Header("Object Poolers")]
    [SerializeField] ObjectPooler baseDemonPooler;
    [SerializeField] ObjectPooler summonerDemonPooler;
    [SerializeField] ObjectPooler stalkerDemonPooler;
    [SerializeField] ObjectPooler choasDemonPooler;
    [SerializeField] ObjectPooler cultistDemonPooler;

    [Header("Demons")]
    private int _base;
    private int _Summoner;
    private int _stalker;
    private int _choas;

    [Header("Timers")]
    private float spawnTimer;
    private float endRoundTimer;

    private Dictionary<DemonID, ObjectPooler> demonPoolers = new Dictionary<DemonID, ObjectPooler>();

    private Queue<DemonType> DemonQueue = new Queue<DemonType>();

    private void Awake()
    {
        demonPoolers.Add(DemonID.Base, baseDemonPooler);
        demonPoolers.Add(DemonID.Summoner, summonerDemonPooler);
        demonPoolers.Add(DemonID.Stalker, stalkerDemonPooler);
        demonPoolers.Add(DemonID.Chaos, choasDemonPooler);
        demonPoolers.Add(DemonID.Cultist, cultistDemonPooler);
    }

    private void Start()
    {
        baseSpawners = HelperFuntions.GetAllChildrenTransformsFromParent(baseSpawner);
        specialSpawners = HelperFuntions.GetAllChildrenTransformsFromParent(SpecialSpawner);

        SetWaves(waves);

        ActiveSpawners(player, baseSpawners, specialSpawners);

        OnWaveStart();
    }

    private void Update()
    {
        Timers();
        Bools();

        if(endRound == true)
        {
            OnWaveEnd();
        }

        if(startRound == true)
        {
            endRoundTimer += Time.deltaTime;
            if(HelperFuntions.TimerGreaterThan(endRoundTimer, timeBetweenRounds))
            {
                OnWaveStart();
                endRoundTimer = 0f;
                startRound = false;
            }
        }

        if (HelperFuntions.TimerGreaterThan(spawnTimer, timeBetweenSpawns) && canSpawn == true)
        {
            if (HelperFuntions.IntGreaterThanOrEqual(maxDemonsAtOnce, currentDemons))
            {
                spawnTimer = 0;

                if (DemonQueue.Count <= 0) // if no demons to spawn return
                {
                    return;
                }

                int toSpawn = maxDemonsAtOnce - currentDemons;
                if(toSpawn <= demonsToSpawnEachTick) { }
                else { toSpawn = demonsToSpawnEachTick; }

                if(maxDemonsToSpawn < toSpawn) { toSpawn = maxDemonsToSpawn; }

                if(toSpawn > 0) ActiveSpawners(player, baseSpawners, specialSpawners); // if demoms to spawn check spawners

                for (int i = 0; i < toSpawn; i++)
                {
                    DemonType demon = DemonQueue.Dequeue();
                    Vector3 pos = Vector3.zero;
                    int temp = -2;

                    if (demon.SpawnType == SpawnType.Basic)
                    {
                        temp = Random.Range(0, baseActiveSpawners.Count);
                        pos = baseActiveSpawners[temp].position;
                    }
                    else if (demon.SpawnType == SpawnType.Special)
                    {
                        temp = Random.Range(0, specialActiveSpawners.Count);
                        pos = specialActiveSpawners[temp].position;
                    }

                    if(temp > -1) SpawnDemon(demon.Id, pos); // spawn using object poolers
                }
            }
        }
    }

    #region SpawningFunctions
    void SpawnDemon(DemonID demon, Vector3 pos) // spawns demon at location
    {
        GameObject demonTemp = demonPoolers[demon].Spawn();

        DemonBase demonBase = demonTemp.GetComponent<DemonBase>();

        demonBase.OnSpawn(player);

        demonTemp.transform.position = pos;

        currentDemons++;
        maxDemonsToSpawn--;
    }
    #endregion

    #region SpawnerFunctions
    void ActiveSpawners(Transform player, List<Transform> baseSpawns, List<Transform> specialSpawns)
    {
        Vector2 pos = new Vector2(player.position.x, player.position.z);

        List<Transform> tempListBase = new List<Transform>();
        List<Transform> tempListspecial = new List<Transform>();

        foreach (Transform bt in baseSpawns)
        {
            Vector2 spawnerPos = new Vector2(bt.position.x, bt.position.z);

            float dist = Vector2.Distance(pos, spawnerPos);

            if (dist < maxSpawningDistance)
            {
                tempListBase.Add(bt);
            }
        }

        foreach (Transform st in specialSpawns)
        {
            Vector2 spawnerPos = new Vector2(st.position.x, st.position.z);

            float dist = Vector2.Distance(pos, spawnerPos);

            if (dist < maxSpawningDistance)
            {
                tempListspecial.Add(st);
            }
        }

        foreach (Transform bt in tempListBase)
        {
            NavMeshPath path = new NavMeshPath();

            playerAgent.CalculatePath(bt.position, path);

            playerAgent.SetPath(path);

            float dist = playerAgent.remainingDistance;

            if (dist < maxPathingDistance)
            {
                if (!baseActiveSpawners.Contains(bt))
                {
                    baseActiveSpawners.Add(bt);
                }
            }
            else
            {
                if (baseActiveSpawners.Contains(bt))
                {
                    baseActiveSpawners.Remove(bt);
                }
            }
        }

        foreach (Transform st in tempListspecial)
        {
            NavMeshPath path = new NavMeshPath();

            playerAgent.CalculatePath(st.position, path);

            playerAgent.SetPath(path);

            float dist = playerAgent.remainingDistance;

            if (dist < maxPathingDistance)
            {
                if (!specialActiveSpawners.Contains(st))
                {
                    specialActiveSpawners.Add(st);
                }
            }
            else
            {
                if (specialActiveSpawners.Contains(st))
                {
                    specialActiveSpawners.Remove(st);
                }
            }
        }
    }
    #endregion

    #region WaveFunctions
    void OnWaveStart()
    {
        wave = GetWave(currentRound);

        maxDemonsToSpawn = (int)demonsToSpawn.Evaluate(currentRound);
        demonsToSpawnEachTick = (int)spawnsEachTick.Evaluate(currentRound);

        _base = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.BasePercentage, maxDemonsToSpawn));
        _Summoner = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.SummonerPercentage, maxDemonsToSpawn));
        _stalker = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.StalkerPercentage, maxDemonsToSpawn));
        _choas = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.ChoasPercentage, maxDemonsToSpawn));

        int temp = maxDemonsToSpawn;

        List<DemonType> DemonsToSpawn = new List<DemonType>();
        List<DemonType> specialDemonTypes = new List<DemonType>();

        for (int i = 0; i < _base; i++)
        {
            DemonsToSpawn.Add(wave.Base);
        }

        temp -= _base;

        for (int i = 0; i < _Summoner; i++)
        {
            specialDemonTypes.Add(wave.Summoner);
        }

        temp -= _Summoner;

        for (int i = 0; i < _stalker; i++)
        {
            specialDemonTypes.Add(wave.Stalker);
        }

        temp -= _stalker;

        for (int i = 0; i < _choas; i++)
        {
            specialDemonTypes.Add(wave.Summoner);
        }

        temp -= _choas;
        maxDemonsToSpawn -= temp;

        specialDemonTypes = HelperFuntions.ShuffleList(specialDemonTypes); // shuffles the special demon list

        int listSize = specialDemonTypes.Count;

        //HelperFuntions.ClearLog();

        for (int i = 0; i < listSize; i++)
        {
            // calculate at what position to add demon
            int index = Mathf.RoundToInt(GetRandomIndexBetweenMinMax(minMax.x, minMax.y, DemonsToSpawn.Count));

            Debug.Log("Index to add: " + index + " Max Size is: " + DemonsToSpawn.Count);

            DemonsToSpawn.Insert(index, specialDemonTypes[i]);
        }

        if (wave.BossWave == true) // add boss at 10% way through
        {
            // calculate at what position to add demon
            int index = GetSpawnIndex(40, maxDemonsToSpawn);

            DemonsToSpawn.Insert(index, wave.Cultist);

            maxDemonsToSpawn++;
        }

        DemonQueue = HelperFuntions.AddListToQueue(DemonsToSpawn);

        startRound = false;
        canSpawn = true;
    }
    void OnWaveEnd()
    {
        DemonQueue.Clear();
        currentRound++;
        canSpawn = false;
        startRound = true;
        endRound = false;
    }
    void SetWaves(List<Wave> list)
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
    Wave GetWave(int currentRound)
    {
        if (currentRound > WavesContainer.Length) return wave = BaseWave;
        return wave = WavesContainer[currentRound];
    }

    void UpdateAllDemonHealth()
    {

    }
    #endregion

    #region Timers
    void Timers()
    {
        spawnTimer += Time.deltaTime;
    }
    #endregion

    #region Bools
    void Bools()
    {
        if (maxDemonsToSpawn <= 0 && currentDemons <= 0 && startRound == false) endRound = true;
    }
    #endregion

    #region SpawnerAccessorFunctions
    public void DemonKilled()
    {
        currentDemons--;
    }
    public void DemonRespawn(DemonType demon)
    {
        currentDemons--;
        maxDemonsToSpawn++;

        DemonQueue.Enqueue(demon);
    }
    #endregion


    int GetSpawnIndex(float percentage, int total)
    {
        return Mathf.RoundToInt(percentage / 100 * total);
    }

    float GetRandomIndexBetweenMinMax(float minPercent, float maxPercent, float total)
    {
        float min = (minPercent / 100) * total;
        float max = (maxPercent / 100) * total;

        return Random.Range(min, max);
    }

}
