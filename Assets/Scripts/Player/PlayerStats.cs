using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour,IDataPersistance<GameData>,IDataPersistance<SessionData>
{
    public float damageMulti = 1;
    public float reloadTimeMulti = 1;
    public float speedMulti = 1;
    public float accelerationMulti = 1;
    public float healthMulti = 1;
    public float regenMulti = 1;
    public float bloodGainMulti = 1;
	public float pointGainMulti = 1;
    public float abilityDamageMulti = 1;

	public int points = 0;
    public int pointsSpent;
    public int pointsGained;
    int kills;
    int headshotKills;
    public int killStreak;
    [Tooltip("For Demon arm")]
    public int maxKillStreak = 15;

    ArmMeshCombiner armMeshCombiner;
    PointGainUi gainUi;

	private void Awake()
	{
        armMeshCombiner = FindObjectOfType<ArmMeshCombiner>();
		gainUi = FindObjectOfType<PointGainUi>();

        //GamePrefs.SetStartMoney(99999);

        points = GamePrefs.StartPoints;
        gainUi.displayPoints = points;
	}

	public void GainPoints(int amount)
	{
        amount = (int)(amount * pointGainMulti);
        points += amount;
        pointsGained += amount;
        gainUi.OnChangePoints(amount);
	}
    public void SpendPoints(int amount)
	{
        points -= amount;
        pointsSpent += amount;
        gainUi.OnChangePoints(-amount);
	}
    public void OnKill(HitBox.BodyPart bodyPart)
    {
        kills++;
        killStreak++;
        armMeshCombiner.UpdateProgress(Mathf.Clamp01((float)kills / maxKillStreak));

        if(bodyPart == HitBox.BodyPart.Head)
            headshotKills++;

    }

    public void ResetKillStreak()
    {
        killStreak = 0;
        armMeshCombiner.UpdateProgress(0);
    }
    void IDataPersistance<GameData>.SaveData(ref GameData data)
	{
        data.pointsSpent += pointsSpent;
        data.pointsGained += pointsGained;
        data.kills += kills;
        data.headShotKills += headshotKills;
	}

    void IDataPersistance<GameData>.LoadData(GameData data)
    {
        //pointsSpent = data.pointsSpent;
        //pointsGained = data.pointsGained;
    }

    public void LoadData(SessionData data)
    {
        //throw new System.NotImplementedException();
    }

    

    public void SaveData(ref SessionData data)
    {
        data.pointsSpent = pointsSpent;
        data.pointsGained = pointsGained;
        data.kills = kills;
        data.headShotKills = headshotKills;
    }
}
