using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantPointsOnDeath : MonoBehaviour
{
    public static float gunGainMod = 1;
    public static float abilityGainMod = 1;
    PlayerStats playerStats;
    public int points;

    // Start is called before the first frame update
    void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        GetComponent<Health>().OnDeath += AddPointsDeath;
    }

    void AddPointsDeath()
	{
        
        playerStats.GainPoints((int)(points * gunGainMod));
	}
}

public enum HitType
{
    gun,
    ability
}
