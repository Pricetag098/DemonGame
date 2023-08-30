using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantPointsOnDeath : MonoBehaviour
{
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
        playerStats.GainPoints(points);
	}
}
