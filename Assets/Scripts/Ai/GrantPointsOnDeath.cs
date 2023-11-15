using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantPointsOnDeath : MonoBehaviour
{
    public static float gunGainMod = 1;
    public static float abilityGainMod = 0.5f;
    PlayerStats playerStats;
    public int points;

    // Start is called before the first frame update
    void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        //GetComponent<Health>().OnDeath += AddPointsDeath;
    }

    public void AddPointsDeathGun()
	{
        playerStats.GainPoints((int)(points * gunGainMod));
	}
    public void AddPointsDeathAbility()
    {
        playerStats.GainPoints((int)(points * abilityGainMod));
    }
}

public enum HitType
{
    Null,
    GUN,
    ABILITY
}
