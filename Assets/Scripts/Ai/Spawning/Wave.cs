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
    [Header("Boss Wave")]
    public bool BossWave;

    [Header("Wave")]
    public SpawnType spawnType;
    public int Round;

    [Header("Demons")]
    public DemonType Base;
    public float BasePercentage;
    public float WalkerPercent;
    public float JoggerPercent;
    public float RunnerPercent;
    public DemonType Summoner;
    public float SummonerPercentage;
    public DemonType Stalker;
    public float StalkerPercentage;
    public DemonType Choas;
    public float ChoasPercentage;
    public DemonType Cultist;
    public float Amount;
}