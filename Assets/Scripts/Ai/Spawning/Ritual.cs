using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ritual", menuName = "Ritual/New Ritual", order = 0)]
public class Ritual : ScriptableObject
{
    [Header("Wave")]
    public Wave ritualWave;

    public int demonsToSpawn;
    public int MaxDemonsAtOnce;
    public int SpawnsPerTick;
    public int TimeBetweenSpawns;

    public bool FinalRitual;
    public bool TpPlayer;
}
