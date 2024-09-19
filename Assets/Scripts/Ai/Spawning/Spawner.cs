using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DemonInfo;

public class Spawner : MonoBehaviour
{
    [SerializeField] private bool SpawnerInMap;
    [SerializeField] private bool ParticleOnSpawn;
    [HideInInspector] public Vector3 position;
    [HideInInspector] public bool CanSpawn;
    [HideInInspector] public bool Visited;
    [HideInInspector] public float distToArea;

    [SerializeField] VfxSpawnRequest spawnRequest;

    public delegate void Action();
    private Action particleSpawnAction;

    Vector3 up;

    private float lightingtime;

    private bool spawnLightning = false;
    private Timer lightningTimer;

    private void Awake()
    {
        position = transform.position;
        up = transform.up;
        CanSpawn = true;

        if(ParticleOnSpawn == true)
        {
            particleSpawnAction += OnSpawn;
        }

        lightningTimer = new Timer(1);
    }

    private void Update()
    {
        if(spawnLightning == true)
        {
            if(lightningTimer.TimeGreaterThan)
            {
                lightningTimer.ResetTimer(lightingtime);
                spawnLightning = false;
            }
        }
    }

    /// <summary>
    /// Return if a Spawner is able to spawn.
    /// </summary>
    /// <param name="demon"></param>
    /// <param name="spawner"></param>
    /// <param name="sm"></param>
    /// <returns></returns>
    public bool RequestSpawn(DemonType demon, SpawnerManager sm, SpawnType type, bool isFinalRitual)
    {
        if(CanSpawn == true)
        {
            SpawnDemon(demon, sm, type, isFinalRitual);
            particleSpawnAction?.Invoke();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Return if a Spawner is able to spawn for ritual
    /// </summary>
    /// <param name="demon"></param>
    /// <param name="sm"></param>
    /// <param name="list"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool RequestSpawn(DemonType demon, SpawnerManager sm, List<DemonFramework> list, SpawnType type, bool isFinalRitual)
    {
        if (CanSpawn == true)
        {
            SpawnDemon(demon, sm, list, type, isFinalRitual);
            particleSpawnAction?.Invoke();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Spawns a Demon
    /// </summary>
    /// <param name="demon"></param>
    /// <param name="pool"></param>
    /// <param name="target"></param>
    private void SpawnDemon(DemonType demon, SpawnerManager sm, SpawnType type, bool isFinalRitual)
    {
        GameObject demonTemp = DemonPoolers.demonPoolers[demon.Id].Spawn();
        DemonFramework demonBase = demonTemp.GetComponent<DemonFramework>();
        demonBase.OnSpawn(demon, sm.player, type, SpawnerInMap, isFinalRitual);
        demonTemp.transform.position = position;
    }

    /// <summary>
    /// Spawns a Demon for a Ritual
    /// </summary>
    /// <param name="demon"></param>
    /// <param name="target"></param>
    /// <param name="list"></param>
    /// <param name="type"></param>
    private void SpawnDemon(DemonType demon, SpawnerManager sm, List<DemonFramework> list, SpawnType type, bool isFinalRitual)
    {
        GameObject demonTemp = DemonPoolers.demonPoolers[demon.Id].Spawn();
        DemonFramework demonBase = demonTemp.GetComponent<DemonFramework>();
        demonBase.OnSpawn(demon, sm.player, type, SpawnerInMap, isFinalRitual);
        demonTemp.transform.position = position;
        list.Add(demonBase);
    }

    private void OnSpawn()
    {
        if(spawnRequest != null)
        {
            spawnRequest.Play(position, up);
            if(spawnRequest.prefab.TryGetComponent<LightningStrike>(out LightningStrike lightningStrike))
            {
                lightningStrike.Play();
                lightingtime = lightningStrike.lastTime;
                spawnLightning = true;
            }
        }
    }
}