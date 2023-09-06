using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualManager : MonoBehaviour
{
    public List<Ritual> Rituals = new List<Ritual>();
    [HideInInspector] public int RitualIndex = 0;
    [HideInInspector] public RitualSpawner currentRitualSpawner;
    [HideInInspector] public Ritual currentRitual;

    [SerializeField] Optional<Transform> finalCompletion;

    private void Awake()
    {
        if (finalCompletion.Enabled)
        {
            finalCompletion.Value.gameObject.SetActive(false);
        }
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
        if (finalCompletion.Enabled && currentRitual.FinalRitual == true)
        {
            finalCompletion.Value.gameObject.SetActive(true);
        }
    }

    public void CurrentRitualOnDemonDeath()
    {
        currentRitualSpawner.currentDemons--;
        currentRitualSpawner.demonsLeft--;
    }
}
