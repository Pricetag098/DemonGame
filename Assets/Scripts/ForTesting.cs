using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForTesting : MonoBehaviour
{
    public float despawnTime;
    public float flashTime;
    public float maxTimeBetweenFlash;
    public float minTimeBetweenFlash;
    public GameObject model;
    PooledObject _pooledObject;
    Timer _timer;
    Timer flashTimer;

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
        else if(_timer.Time > flashTime)
        {
            StartFlashTimer();
        }
    }

    public void FlashModel()
    {
        if (model.activeSelf)
        {
            model.SetActive(false);
        }
        else
        {
            model.SetActive(true);
        }
    }

    void StartFlashTimer()
    {
        if (flashTimer.TimeGreaterThan)
        {
            FlashModel();
            flashTimer.ResetTimer(NewFlashTime(_timer));
        }
    }

    float NewFlashTime(Timer timer)
    {
        float closeness = timer.Time / despawnTime;
        closeness = Mathf.Lerp(maxTimeBetweenFlash, minTimeBetweenFlash, closeness);
        return closeness;
    }

    private void OnEnable()
    {
        flashTimer = new Timer(maxTimeBetweenFlash);
        _timer = new Timer(despawnTime);

        model.SetActive(true);
    }
}
