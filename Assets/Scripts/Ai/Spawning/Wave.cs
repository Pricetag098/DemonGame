using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonInfo;

/// <summary>
/// percentage based spawning
/// </summary>

[CreateAssetMenu(fileName = "Wave", menuName = "Wave/Create Wave", order = 0)]
public class Wave : ScriptableObject
{
    [Header("Wave")]
    public SpawnType spawnType;
    public int Round;

    [Header("Demons")]
    public DemonType Base;
    public float BasePercentage;
    public float WalkerPercent;
    public float JoggerPercent;
    public float RunnerPercent;

    public DemonType LittleGuy;
    public float LittleGuyPercentage;

    public DemonType Choas;
    public int ChoasAmount;

    [Header("Extra Chaos Chance")]
    [Range(0,100)] public float ChanceToSpawnChaos;
    [Range(0,10)] public int MaxChoasToSpawn;
}