using DemonInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public WaveManager WaveManager;
    public DemonSpawner DemonSpawner;
    public DemonPoolers DemonPoolers;

    [Header("Spawning Stats")]
    public bool canSpawn;
    [SerializeField] int maxDemonsAtOnce;
    public float timeBetweenSpawns;
    [SerializeField] float timeBetweenRounds;
    public int demonsToSpawnEachTick;
    public Vector2Int minMax;

    [Header("Display Stats")]
    public int maxDemonsToSpawn;
    public int currentDemons;

    private void Awake()
    {
        WaveManager = GetComponent<WaveManager>();
        DemonSpawner = GetComponent<DemonSpawner>();
        DemonPoolers = GetComponent<DemonPoolers>();
    }
    private void Start()
    {
        
    }
}
