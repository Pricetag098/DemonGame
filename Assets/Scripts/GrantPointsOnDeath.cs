using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantPointsOnDeath : MonoBehaviour
{
    public static float gainMod = 1;
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
        playerStats.GainPoints((int)(points * gainMod));
	}
}
