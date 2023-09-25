using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer
{
    [SerializeField] private float _timer;
    [SerializeField] private float _timeInterval;
    public Timer()
    {
        _timer = 0;
        _timeInterval = 0;
    }

    public Timer(float timeInterval)
    {
        _timer = 0;
        _timeInterval = timeInterval;
    }
    public Timer(float timeInterval, bool triggerFristFrame = false)
    {
        if(triggerFristFrame == false)
        {
            _timer = 0;
            _timeInterval = timeInterval;
        }
        else
        {
            _timer = timeInterval;
            _timeInterval = timeInterval;
        }
    }

    public bool TimerGreaterThan(out float time)
    {
        if (_timer > _timeInterval)
        {
            _timer = 0;
            time = _timer;
            return true;
        }

        _timer += Time.deltaTime;
        time = _timer;
        return false;
    }

    public void ResetTimer(float amount)
    {
        _timeInterval = amount;
    }

    public bool TimeGreaterThan
    {
        get 
        {
            if (_timer >= _timeInterval)
            {
                _timer = 0;
                return true;
            }

            _timer += Time.deltaTime;
            return false;
        }
    }
}
