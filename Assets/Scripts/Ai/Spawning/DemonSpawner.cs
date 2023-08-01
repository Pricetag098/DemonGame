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

    [Header("Spawning Stats")]
    [SerializeField] int maxDemonsToSpawn;
    [SerializeField] int maxDemonsAtOnce;
    [SerializeField] int currentDemons;
    [SerializeField] float maxSpawningDistance;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] int demonsToSpawnEachTick;

    [SerializeField] bool canSpawn;

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

    [Header("Timers")]
    float spawnTimer;

    private Dictionary<DemonID, ObjectPooler> demonPoolers = new Dictionary<DemonID, ObjectPooler>();

    private Queue<DemonID> DemonQueue = new Queue<DemonID>();

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

        maxDemonsToSpawn = (int)demonsToSpawn.Evaluate(currentRound);
        demonsToSpawnEachTick = (int)spawnsEachTick.Evaluate(currentRound);
    }

    private void Update()
    {
        Timers();

        //OnWaveEnd();
        //OnWaveStart(wave);

        if(HelperFuntions.TimerGreaterThan(spawnTimer, timeBetweenSpawns) && canSpawn == true)
        {
            if (HelperFuntions.IntGreaterThanOrEqual(maxDemonsAtOnce, currentDemons))
            {
                spawnTimer = 0;

                if (maxDemonsToSpawn <= 0)
                {
                    canSpawn = false;
                    return;
                }

                int toSpawn = maxDemonsAtOnce - currentDemons;
                if(toSpawn <= demonsToSpawnEachTick) { }
                else { toSpawn = demonsToSpawnEachTick; }

                if(maxDemonsToSpawn < toSpawn) { toSpawn = maxDemonsToSpawn; }

                Debug.Log("Amount of Demons To Spawn: " + toSpawn);

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

    int GetDemonSpawnChance(float min, float max) // calculates each types spawn chance
    {
        float chance = Random.Range(min, max);

        if(chance > 0)
        {
            float spawnFloat = 0;
            spawnFloat = maxDemonsToSpawn * chance;
            return Mathf.FloorToInt(spawnFloat);
        }

        return 0;
    }

    void AddListToQueue(Queue q, List<DemonID> list)
    {
        foreach(DemonID item in list) 
        {
            q.Enqueue(item);
        }
    }

    void OnWaveStart(Wave currentwave)
    {
        wave = currentwave;
        currentRound++;

        maxDemonsAtOnce = (int)demonsToSpawn.Evaluate(currentRound);
        demonsToSpawnEachTick = (int)spawnsEachTick.Evaluate(currentRound);

        // create and set the demon queue



        canSpawn = true;
    }

    void OnWaveEnd()
    {
        DemonQueue.Clear();
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
