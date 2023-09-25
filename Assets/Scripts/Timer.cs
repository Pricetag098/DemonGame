using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer
{
    public float _timer { get; private set; }
    public float _timeInterval { get; private set; }

    public Timer(float timeInterval)
    {
        _timer = 0;
        _timeInterval = timeInterval;
    }
    public Timer(float timeInterval, bool triggerFristFrame = false)
    {
        if(triggerFristFrame == true)
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

    public void SetTimeInterval(float amount)
    {
        _timeInterval = amount;
    }

    public void SetTime(float time)
    {
        _timer = time;
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
