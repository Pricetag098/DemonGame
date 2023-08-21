using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonInfo;
using System.Reflection;
using Unity.Jobs;
using UnityEngine.AI;

public class DemonSpawner : MonoBehaviour
{
    [Header("Waves")]
    [SerializeField] WaveManager waveManager; 
    public delegate void Action();
    public Action OnWaveEnd;
    public Action OnWaveStart;

    public bool endRound;
    public bool startRound;

    [Header("Animation Curves")]
    public AnimationCurve demonsToSpawn;
    public AnimationCurve spawnsEachTick;

    [Header("Timers")]
    private float spawnTimer;
    private float endRoundTimer;

    [HideInInspector] public Queue<DemonType> DemonQueue = new Queue<DemonType>();

    private void Awake()
    {
        waveManager = GetComponent<WaveManager>();
    }

    private void Start()
    {
        OnWaveEnd += WaveEnd;
        OnWaveStart.Invoke();
    }
    //private void Update()
    //{
    //    Timers();
    //    Bools();

    //    if (endRound == true)
    //    {
    //        OnWaveEnd.Invoke();
    //    }

    //    if (startRound == true)
    //    {
    //        endRoundTimer += Time.deltaTime;
    //        if (HelperFuntions.TimerGreaterThan(endRoundTimer, timeBetweenRounds))
    //        {
    //            OnWaveStart.Invoke();
    //            endRoundTimer = 0f;
    //            startRound = false;
    //        }
    //    }

    //    if (HelperFuntions.TimerGreaterThan(spawnTimer, timeBetweenSpawns) && canSpawn == true)
    //    {
    //        if (HelperFuntions.IntGreaterThanOrEqual(maxDemonsAtOnce, currentDemons))
    //        {
    //            spawnTimer = 0;

    //            if (DemonQueue.Count <= 0) // if no demons to spawn return
    //            {
    //                return;
    //            }

    //            int toSpawn = maxDemonsAtOnce - currentDemons;
    //            if (toSpawn <= demonsToSpawnEachTick) { }
    //            else { toSpawn = demonsToSpawnEachTick; }

    //            if (maxDemonsToSpawn < toSpawn) { toSpawn = maxDemonsToSpawn; }

    //            //if (toSpawn > 0) ActiveSpawners(player, baseSpawners, specialSpawners); // if demoms to spawn check spawners

    //            for (int i = 0; i < toSpawn; i++)
    //            {
    //                DemonType demon = null;
    //                if (DemonQueue.Count > 0) { demon = DemonQueue.Dequeue(); }
    //                else { break; }

    //                Spawner spawner = null;
    //                int temp = -2;

    //                // go through all avalible spawners once all been used refil spawner list

    //                if (demon.SpawnType == SpawnType.Basic)
    //                {
    //                    temp = Random.Range(0, baseActiveSpawners.Count);
    //                    spawner = baseActiveSpawners[temp];
    //                }
    //                else if (demon.SpawnType == SpawnType.Special)
    //                {
    //                    temp = Random.Range(0, specialActiveSpawners.Count);
    //                    spawner = specialActiveSpawners[temp];
    //                }

    //                if (temp > -1) spawner.RequestSpawn(demon); // spawn using object poolers
    //            }
    //        }
    //    }
    //}

    void Timers()
    {
        spawnTimer += Time.deltaTime;
    }

    //void Bools()
    //{
    //    if (maxDemonsToSpawn <= 0 && currentDemons <= 0 && startRound == false) endRound = true;
    //}
    public void WaveEnd()
    {
        DemonQueue.Clear();
        startRound = true;
        endRound = false;
    }

    #region SpawnerAccessorFunctions
    //public void DemonKilled()
    //{
    //    currentDemons--;
    //}

    //public void DemonRespawn(DemonType demon)
    //{
    //    currentDemons--;
    //    maxDemonsToSpawn++;

    //    DemonQueue.Enqueue(demon);
    //}
    #endregion


    

    private void OnGUI()
    {
        //string content = currentRound.ToString();
        //GUILayout.Label($"<color='white'><size=150>{content}</size></color>");
    }
}
