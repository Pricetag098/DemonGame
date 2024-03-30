using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlessingType
{
    Null,
    BulletHell,
    DivineSmite,
    MaxBlood,
    Carpenter,
    DoublePoints,
    InstaKill,
    MaxAmmo
}

public class BlessingSpawner : MonoBehaviour
{
    [SerializeField] ObjectPooler bulletHell;
    [SerializeField] ObjectPooler carpenter;
    [SerializeField] ObjectPooler divineSmite;
    [SerializeField] ObjectPooler doublePoints;
    [SerializeField] ObjectPooler fillBlood;
    [SerializeField] ObjectPooler instaKill;
    [SerializeField] ObjectPooler maxAmmo;

    BlessingType lastBlessing;

    private Dictionary<BlessingType, ObjectPooler> blessingPooler = new Dictionary<BlessingType, ObjectPooler>();

    private void Awake()
    {
        blessingPooler.Add(BlessingType.BulletHell, bulletHell);
        blessingPooler.Add(BlessingType.Carpenter, carpenter);
        blessingPooler.Add(BlessingType.DivineSmite, divineSmite);
        blessingPooler.Add(BlessingType.DoublePoints, doublePoints);
        blessingPooler.Add(BlessingType.MaxBlood, fillBlood);
        blessingPooler.Add(BlessingType.InstaKill, instaKill);
        blessingPooler.Add(BlessingType.MaxAmmo, maxAmmo);
    }

    private BlessingType GetRandomEnum()
    {
        BlessingType newBlessing = (BlessingType)Random.Range(1, System.Enum.GetValues(typeof(BlessingType)).Length);
        while (newBlessing == lastBlessing)
        {
            newBlessing = (BlessingType)Random.Range(1, System.Enum.GetValues(typeof(BlessingType)).Length);
        }
        lastBlessing = newBlessing;
        return newBlessing;
    }

    public void SpawnBlessing(Transform spawnPos, BlessingType type = BlessingType.Null)
    {
        if (type == BlessingType.Null) { type = GetRandomEnum(); }

        GameObject blessing = blessingPooler[type].Spawn();
        blessing.transform.position = spawnPos.position;
    }
}
