using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour,IDataPersistance
{
    public float damageMulti = 1;
    public float reloadTimeMulti = 1;
    public float speedMulti = 1;
    public float accelerationMulti = 1;
    public float healthMulti = 1;
    public float regenMulti = 1;
    public float bloodGainMulti = 1;

    public int points = 0;
    public int pointsSpent;
    public int pointsGained;

    public void GainPoints(int amount)
	{
        points += amount;
        pointsGained += amount;
	}
    public void SpendPoints(int amount)
	{
        points -= amount;
        pointsSpent += amount;
	}
    void IDataPersistance.SaveData(ref GameData data)
	{
        data.pointsSpent = pointsSpent;
        data.pointsGained = pointsGained;
	}

    void IDataPersistance.LoadData(GameData data)
    {
        pointsSpent = data.pointsSpent;
        pointsGained = data.pointsGained;
    }
}
