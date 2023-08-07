using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonCum;

/// <summary>
/// percentage based spawning
/// </summary>

[CreateAssetMenu(fileName = "Wave", menuName = "Wave/Create Wave", order = 0)]
public class Wave : ScriptableObject
{
    [Header("Boss Wave")]
    public bool BossWave;

    [Header("Wave id")]
    public int Round;

    [Header("Demons")]
    public DemonType Base;
    public float BasePercentage;
    public DemonType Summoner;
    public float SummonerPercentage;
    public DemonType Stalker;
    public float StalkerPercentage;
    public DemonType Choas;
    public float ChoasPercentage;
    public DemonType Cultist;
    public float Amount;
}