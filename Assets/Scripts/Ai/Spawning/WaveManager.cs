using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private SpawnerManager Manager;

    [SerializeField] Wave wave;
    [SerializeField] Wave BaseWave;
    [SerializeField] Wave BossWave;

    [SerializeField] int BossWaveIncrement;
    [SerializeField] int IgnoreFirstIncrement;

    [SerializeField] List<Wave> waves = new List<Wave>();
    private Wave[] WavesContainer = new Wave[101];

    [Header("Demons")]
    private int _base;
    private int _littleGuy;

    private void Awake()
    {
        Manager = GetComponent<SpawnerManager>();
        SetWaves(waves);
    }

    /// <summary>
    /// Sets the Current wave To the Next wave
    /// </summary>
    /// <param name="currentRound"></param>
    public void NextWave(int currentRound)
    {
        wave = GetWave(currentRound);
    }

    /// <summary>
    /// Returns Demons To Add to Queue<DemonType> from Current Wave 
    /// </summary>
    /// <param name="MaxToSpawn"></param>
    /// <returns></returns>
    public Queue<DemonType> GetDemonToSpawn(int MaxToSpawn)
    {
        _base = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.BasePercentage, MaxToSpawn));

        int jogger = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.JoggerPercent, _base));
        int runner = Mathf.RoundToInt(HelperFuntions.GetPercentageOf(wave.RunnerPercent, _base));

        jogger += runner;

        List<DemonType> DemonsToSpawn = new List<DemonType>();

        for (int i = 0; i < _base; i++)
        {
            DemonType tempBase = new DemonType();
            tempBase.SpawnType = wave.Base.SpawnType;
            tempBase.Id = wave.Base.Id;
            tempBase.SpawnerType = wave.Base.SpawnerType;

            if(i < runner) { tempBase.SpeedType = DemonInfo.SpeedType.Runner; }
            else if(i < jogger) { tempBase.SpeedType = DemonInfo.SpeedType.Jogger; }
            else { tempBase.SpeedType = DemonInfo.SpeedType.Walker; }

            DemonsToSpawn.Add(tempBase);
        }

        DemonsToSpawn = HelperFuntions.ShuffleList(DemonsToSpawn);

        for (int i = 0; i < wave.WavePositions.Count; i++)
        {
            DemonsToSpawn.Insert(GetSpawnIndex(wave.WavePositions[i], DemonsToSpawn.Count), wave.Choas);
        }

        Manager.maxDemonsToSpawn = DemonsToSpawn.Count;

        //Debug.Log("Demons to Spawn This wave: " + MaxToSpawn + " How many are in queue: " + DemonsToSpawn.Count);

        Manager.StartOfRound = false;
        Manager.canSpawn = true;

        return HelperFuntions.AddListToQueue(DemonsToSpawn);
    }

    /// <summary>
    /// Initalises All Waves
    /// </summary>
    /// <param name="list"></param>
    private void SetWaves(List<Wave> list)
    {
        foreach (Wave w in list)
        {
            if (w.Round != 0)
            {
                WavesContainer[w.Round] = w;
            }
        }

        int count = WavesContainer.Length;

        for (int i = 0; i < count; i++)
        {
            if (i % BossWaveIncrement == 0 && i != IgnoreFirstIncrement)
            {
                WavesContainer[i] = BossWave;
            }
        }

        for (int i = 1; i < count; i++) // set all left over rounds as base rounds
        {
            if (WavesContainer[i] == null)
            {
                WavesContainer[i] = BaseWave;
            }
        }
    }
    public int GetSpawnIndex(float percentage, int total)
    {
        return Mathf.RoundToInt(percentage / 100 * total);
    }

    /// <summary>
    /// Returns a Wave from the Array of Waves
    /// </summary>
    /// <param name="currentRound"></param>
    /// <returns></returns>
    public Wave GetWave(int currentRound)
    {
        if (currentRound >= WavesContainer.Length) return wave = BaseWave;
        return wave = WavesContainer[currentRound];
    }
}
