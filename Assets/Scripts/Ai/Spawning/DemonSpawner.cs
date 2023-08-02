using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonCum;

public class DemonSpawner : MonoBehaviour
{
    [SerializeField] Transform player;

    [Header("Wave")]
    public int currentRound;
    [SerializeField] Wave wave;

    [Header("Display Stats")]
    [SerializeField] int maxDemonsToSpawn;
    [SerializeField] int currentDemons;

    [Header("Spawning Stats")]
    [SerializeField] bool canSpawn;
    [SerializeField] int maxDemonsAtOnce;
    [SerializeField] float maxSpawningDistance;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] int demonsToSpawnEachTick;
    [SerializeField] Vector2Int minMax;

    [Header("Animation Curves")]
    [SerializeField] AnimationCurve demonsToSpawn;
    [SerializeField] AnimationCurve spawnsEachTick;

    [Header("Spawn Location")]
    [SerializeField] Transform baseSpawner;
    [SerializeField] List<Transform> baseActiveSpawners = new List<Transform>();
    [SerializeField] private List<Transform> baseSpawners = new List<Transform>();

    [SerializeField] Transform SpecialSpawner;
    [SerializeField] List<Transform> specialActiveSpawners = new List<Transform>();
    [SerializeField] private List<Transform> specialSpawners = new List<Transform>();

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
    private int _cultist;

    [Header("Timers")]
    private float spawnTimer;

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
        AddChildrenToList(baseSpawner, baseSpawners);
        AddChildrenToList(SpecialSpawner, specialSpawners);

        ActiveSpawners(player, baseSpawners, specialSpawners);

        OnWaveStart(wave);
    }

    private void Update()
    {
        Timers();

        //OnWaveEnd();
        //OnWaveStart(wave);

        if (HelperFuntions.TimerGreaterThan(spawnTimer, timeBetweenSpawns) && canSpawn == true)
        {
            if (HelperFuntions.IntGreaterThanOrEqual(maxDemonsAtOnce, currentDemons))
            {
                spawnTimer = 0;

                //if (maxDemonsToSpawn <= 0)
                //{
                //    canSpawn = false;
                //    return;
                //}

                int toSpawn = maxDemonsAtOnce - currentDemons;
                if(toSpawn <= demonsToSpawnEachTick) { }
                else { toSpawn = demonsToSpawnEachTick; }

                if(maxDemonsToSpawn < toSpawn) { toSpawn = maxDemonsToSpawn; }

                //Debug.Log("Amount of Demons To Spawn: " + toSpawn);

                ActiveSpawners(player, baseSpawners, specialSpawners);

                for (int i = 0; i < toSpawn; i++)
                {
                    DemonType dt = DemonQueue.Dequeue();
                    Vector3 pos = Vector3.zero;

                    if (dt.SpawnType == SpawnType.Basic)
                    {
                        int temp = Random.Range(0, baseActiveSpawners.Count);
                        pos = baseActiveSpawners[temp].position;
                    }
                    else if(dt.SpawnType == SpawnType.Special)
                    {
                        int temp = Random.Range(0, specialActiveSpawners.Count);
                        pos = specialActiveSpawners[temp].position;
                    }

                    // spawn using object poolers
                    SpawnDemon(dt.Id, pos); // place holder spawn location
                }
            }
        }
    }

    void OnWaveStart(Wave currentwave)
    {
        wave = currentwave;
        currentRound++;

        maxDemonsToSpawn = (int)demonsToSpawn.Evaluate(currentRound);
        demonsToSpawnEachTick = (int)spawnsEachTick.Evaluate(currentRound);

        _base = Mathf.RoundToInt(GetDemonSpawnChance(wave.Base.Percentage, maxDemonsToSpawn));
        _Summoner = Mathf.RoundToInt(GetDemonSpawnChance(wave.Summoner.Percentage, maxDemonsToSpawn));
        _stalker = Mathf.RoundToInt(GetDemonSpawnChance(wave.Stalker.Percentage, maxDemonsToSpawn));
        _choas = Mathf.RoundToInt(GetDemonSpawnChance(wave.Choas.Percentage, maxDemonsToSpawn));

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

        for (int i = 0; i < listSize; i++)
        {
            // calculate at what position to add demon
            int min = Mathf.RoundToInt(GetDemonSpawnChance(minMax.x, DemonsToSpawn.Count));
            int max = Mathf.RoundToInt(GetDemonSpawnChance(minMax.x, minMax.y));
            int index = Random.Range(min, max);

            DemonsToSpawn.Insert(index, specialDemonTypes[i]);
        }

        AddListToQueue(DemonQueue, DemonsToSpawn);

        canSpawn = true;
    }

    void OnWaveEnd()
    {
        DemonQueue.Clear();
        canSpawn = false;
    }

    #region Propterties

    bool EndRound
    {
        get { return maxDemonsToSpawn <= 0 && currentDemons <= 0; }
    }

    #endregion

    public void DemonKilled()
    {
        currentDemons--;
    }

    public void DemonRespawn()
    {
        currentDemons--;
        maxDemonsToSpawn++;
    }

    void SpawnDemon(DemonID demon, Vector3 pos) // spawns demon at location
    {
        GameObject demonTemp = demonPoolers[demon].Spawn();

        DemonBase demonBase = demonTemp.GetComponent<DemonBase>();

        demonBase.OnSpawn(player);

        demonTemp.transform.position = pos;

        currentDemons++;
        maxDemonsToSpawn--;
    }

    float GetDemonSpawnChance(float percentage, int maxDemons)
    {
        return (percentage / 100) * maxDemons;
    }

    void AddListToQueue(Queue<DemonType> q, List<DemonType> list)
    {
        foreach(DemonType item in list) 
        {
            q.Enqueue(item);
        }
    }

    void Timers()
    {
        spawnTimer += Time.deltaTime;
    }

    void AddChildrenToList(Transform parent, List<Transform> list)
    {
        foreach(Transform child in parent)
        {
            list.Add(child);
        }
    }

    void AddSpawnersToActiveSpawners(Transform parent, List<Transform> spawnPoints)
    {
        foreach(Transform t in parent)
        {
            spawnPoints.Add(t);
        }
    }

    void ActiveSpawners(Transform player, List<Transform> baseSpawns, List<Transform> specialSpawns)
    {
        Transform p = player;
        Vector2 playerPos = new Vector2(p.position.x, p.position.z);

        foreach(Transform bt in baseSpawns)
        {
            Vector2 spawnerPos = new Vector2(bt.position.x, bt.position.z);

            float dist = Vector2.Distance(playerPos, spawnerPos);

            if(dist < maxSpawningDistance)
            {
                if(!baseActiveSpawners.Contains(bt))
                {
                    baseActiveSpawners.Add(bt);
                }
            }
            else
            {
                if(baseActiveSpawners.Contains(bt))
                {
                    baseActiveSpawners.Remove(bt);
                }
            }
        }

        foreach(Transform st in specialSpawns)
        {
            Vector2 spawnerPos = new Vector2(st.position.x, st.position.z);

            float dist = Vector2.Distance(playerPos, spawnerPos);

            if (dist < maxSpawningDistance)
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
}
