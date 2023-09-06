using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualManager : MonoBehaviour
{
    public List<Ritual> Rituals = new List<Ritual>();
    [HideInInspector] public int RitualIndex = 0;
    [HideInInspector] public RitualSpawner currentRitualSpawner;
    [HideInInspector] public Ritual currentRitual;

    [SerializeField] List<GameObject> FinalCompletionObjects = new List<GameObject>();
    [SerializeField] Optional<Transform> playerTpLocationStart;
    [SerializeField] Optional<Transform> playerTpLocationEnd;
    

    private Transform player;

    private void Awake()
    {
        foreach(GameObject g in FinalCompletionObjects)
        {
            g.SetActive(false);
        }

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
        if (currentRitual.FinalRitual == true)
        {
            foreach (GameObject g in FinalCompletionObjects)
            {
                g.SetActive(true);
            }
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
}
