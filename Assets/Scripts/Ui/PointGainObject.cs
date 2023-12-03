using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGainObject : MonoBehaviour
{

    private void Awake()
    {
        GetComponent<PooledObject>().OnDespawn += AddPoints;
    }

    void AddPoints()
    {
        
        DOTween.Kill(this, true);
    }
}
