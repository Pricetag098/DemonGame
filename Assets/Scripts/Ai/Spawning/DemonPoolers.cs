using DemonInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonPoolers : MonoBehaviour
{
    [Header("Object Poolers")]
    [SerializeField] ObjectPooler baseDemonPooler;
    [SerializeField] ObjectPooler choasDemonPooler;
    [SerializeField] ObjectPooler stalkerDemonPooler;

    static public Dictionary<DemonID, ObjectPooler> demonPoolers;

    private void Awake()
    {
        demonPoolers = new Dictionary<DemonID, ObjectPooler>();
        demonPoolers.Add(DemonID.Base, baseDemonPooler);
        demonPoolers.Add(DemonID.Chaos, choasDemonPooler);
        demonPoolers.Add(DemonID.LittleGuy, stalkerDemonPooler);
    }
}
