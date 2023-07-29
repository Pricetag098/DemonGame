using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantPointsOnDeath : MonoBehaviour
{
    public PlayerStats playerStats;
    public int points;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Health>().OnDeath += AddPoints;
    }

    void AddPoints()
	{
        playerStats.points += points;
	}
}
