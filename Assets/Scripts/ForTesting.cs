using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForTesting : MonoBehaviour
{
    public float despawnTime;
    PooledObject _pooledObject;
    Timer _timer;

    private void Start()
    {
        _pooledObject = GetComponent<PooledObject>();
    }

    private void Update()
    {
        if (_timer.TimeGreaterThan)
        {
            _pooledObject.Despawn();
        }
    }

    private void OnEnable()
    {
        _timer = new Timer(despawnTime);
    }
}
