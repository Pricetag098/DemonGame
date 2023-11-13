using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitmarkers : MonoBehaviour
{
    [SerializeField] ObjectPooler baseMarkers, critMarkers;
    private void Awake()
    {
        FindObjectOfType<Holster>().OnDealDamage += OnHit;
    }


    void OnHit(float damage,HitBox hitBox)
    {

    }
}
