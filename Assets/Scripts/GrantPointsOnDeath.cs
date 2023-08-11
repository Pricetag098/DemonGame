using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantPointsOnDeath : MonoBehaviour
{
    PlayerStats playerStats;
    public int points;
    public int pointsHit;
    // Start is called before the first frame update
    void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        GetComponent<Health>().OnDeath += AddPointsDeath;
        GetComponent<Health>().OnHit += AddPointsHit;
    }

    void AddPointsDeath()
	{
        playerStats.points += points;
	}
    void AddPointsHit()
    {
        playerStats.points += points;
    }
}
