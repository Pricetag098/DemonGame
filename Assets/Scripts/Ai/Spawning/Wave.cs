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
    public DemonType Summoner;
    public DemonType Stalker;
    public DemonType Choas;
    public DemonType Cultist;
}