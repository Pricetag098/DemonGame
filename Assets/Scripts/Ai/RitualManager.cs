using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RitualManager : MonoBehaviour
{
    public List<Ritual> Rituals = new List<Ritual>();
    [HideInInspector] public int RitualIndex = 0;
    [HideInInspector] public RitualSpawner currentRitualSpawner;
    [HideInInspector] public Ritual currentRitual;

    [SerializeField] UnityEvent FinalCompletionObjects;
    [SerializeField] Optional<Transform> playerTpLocationStart;
    [SerializeField] Optional<Transform> playerTpLocationEnd;

    private Transform player;

    private void Awake()
    {
        player = GetComponent<SpawnerManager>().player;
    }

    public Ritual GetCurrentRitual()
    {
        return currentRitual = Rituals[RitualIndex];
    }

    public void IncrementIndex()
    {
        RitualIndex++;
    }

    public void SetCurrentRitual(RitualSpawner rs)
    {
        currentRitualSpawner = rs;
    }

    public void FinalRitual()
    {
        if(currentRitual is null) { return; }
        
        if (currentRitual.FinalRitual == true)
        {
            FinalCompletionObjects.Invoke();
        }
    }

    public void CurrentRitualOnDemonDeath()
    {
        currentRitualSpawner.currentDemons--;
        currentRitualSpawner.demonsLeft--;
    }

    public void TpPlayerOnStart()
    {
        if(playerTpLocationStart.Enabled && currentRitual.TpPlayer == true)
        {
            player.position = playerTpLocationStart.Value.position;
        }
    }
    public void TpPlayerOnEnd()
    {
        if (playerTpLocationEnd.Enabled && currentRitual.TpPlayer == true)
        {
            player.position = playerTpLocationEnd.Value.position;
        }
    }

    public void AddDemonBackToRitual(DemonType type)
    {
        if (currentRitual is null) { return; }

        currentRitualSpawner.AddDemonBackToQueue(type);
    }
}
