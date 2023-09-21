using DemonInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonPoolers : MonoBehaviour
{
    [Header("Object Poolers")]
    [SerializeField] ObjectPooler baseDemonPooler;
    [SerializeField] ObjectPooler summonerDemonPooler;
    [SerializeField] ObjectPooler stalkerDemonPooler;
    [SerializeField] ObjectPooler choasDemonPooler;
    [SerializeField] ObjectPooler cultistDemonPooler;

    static public Dictionary<DemonID, ObjectPooler> demonPoolers = new Dictionary<DemonID, ObjectPooler>();

    private void Awake()
    {
        demonPoolers.Add(DemonID.Base, baseDemonPooler);
        demonPoolers.Add(DemonID.Stalker, stalkerDemonPooler);
        demonPoolers.Add(DemonID.Chaos, choasDemonPooler);
    }
}
