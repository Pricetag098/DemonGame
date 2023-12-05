using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.HighDefinition;
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

    public Queue<DemonType> DemonWave
    {
        get
        {
            if (DemonWave != null)
            {
                Debug.Log("Wasnt null return value");
                return DemonWave;
            }

            Debug.Log("Was null returning wave");
            return DemonWave = Wave.GetWave(demonsToSpawn, ritualWave);
        }

        private set
        {
            DemonWave = value;
        }
    }
}
