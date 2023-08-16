using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData 
{
	public int kills;
	public int pointsGained;
	public int pointsSpent;

	public GameData()
	{
		kills = 0;
		pointsGained = 0;
		pointsSpent = 0;

	}
}
