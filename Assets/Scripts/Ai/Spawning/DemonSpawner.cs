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

    private Queue<DemonID> DemonQueue = new Queue<DemonID>();
    private Queue<DemonType> DemonQueue2 = new Queue<DemonType>();

    private void Awake()
    {
        demonPoolers.Add(DemonID.Base, baseDemonPooler);
        demonPoolers.Add(DemonID.Summoner, summonerDemonPooler);
        demonPoolers.Add(DemonID.Stalker, stalkerDemonPooler);
        demonPoolers.Add(DemonID.Chaos, choasDemonPooler);
        demonPoolers.Add(DemonID.Cultist, cultistDemonPooler);

        for (int i = 0; i < 200; i++)
        {
            DemonQueue.Enqueue(DemonID.Base);
        }

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
                    // logic for getting which spawner
                    int temp = Random.Range(0, baseActiveSpawners.Count);
                    Vector3 pos = baseActiveSpawners[temp].position;

                    // spawn using object poolers
                    SpawnDemon(DemonQueue.Dequeue(), pos); // place holder spawn location
                }
            }
        }
    }

    public List<DemonType> test = new List<DemonType>();
    public List<DemonType> test2 = new List<DemonType>();

    void OnWaveStart(Wave currentwave)
    {
        wave = currentwave;
        currentRound++;

        maxDemonsToSpawn = (int)demonsToSpawn.Evaluate(currentRound);
        demonsToSpawnEachTick = (int)spawnsEachTick.Evaluate(currentRound);

        // create and set the demon queue
        _base = Mathf.RoundToInt(GetDemonSpawnChance(wave.Base.Percentage, maxDemonsToSpawn));
        _Summoner = Mathf.RoundToInt(GetDemonSpawnChance(wave.Summoner.Percentage, maxDemonsToSpawn));
        _stalker = Mathf.RoundToInt(GetDemonSpawnChance(wave.Stalker.Percentage, maxDemonsToSpawn));
        _choas = Mathf.RoundToInt(GetDemonSpawnChance(wave.Choas.Percentage, maxDemonsToSpawn));

        int temp = maxDemonsToSpawn;

        List<DemonType> baseDemonTypes = new List<DemonType>();
        List<DemonType> specialDemonTypes = new List<DemonType>();

        for (int i = 0; i < _base; i++)
        {
            test.Add(wave.Base);
        }

        temp -= _base;

        for (int i = 0; i < _Summoner; i++)
        {
            test2.Add(wave.Summoner);
        }

        temp -= _Summoner;

        for (int i = 0; i < _stalker; i++)
        {
            test2.Add(wave.Stalker);
        }

        temp -= _stalker;

        for (int i = 0; i < _choas; i++)
        {
            test2.Add(wave.Summoner);
        }

        temp -= _choas;
        maxDemonsToSpawn -= temp;

        HelperFuntions.ShuffleList(test2); // shuffles the special demon list

        // add special demon list to the baseDemonList
        int listSize = specialDemonTypes.Count;

        for (int i = 0; i < listSize; i++)
        {
            // calculate at what position to add demon
            int min = Mathf.RoundToInt(GetDemonSpawnChance(minMax.x, test.Count));
            int max = Mathf.RoundToInt(GetDemonSpawnChance(minMax.y, test.Count));
            int index = Random.Range(min, max);

            test.Insert(index, test2[i]);
        }

        // add list of demons to the queue
        // once list has been shuffled and arranged
        AddListToQueue(DemonQueue2, baseDemonTypes);

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
