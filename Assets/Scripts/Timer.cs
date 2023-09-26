using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Blakes Timer Class
/// </summary>
[System.Serializable]
public class Timer
{
    public delegate void Action();

    /// <summary>
    /// Current Time
    /// </summary>
    public float Time { get; private set; }

    /// <summary>
    /// Time Greater than Interval
    /// </summary>
    public float TimeInterval { get; private set; }

    /// <summary>
    /// If True Timer Will Pause
    /// </summary>
    public bool Pause { get; private set; }

    /// <summary>
    /// Delegate Called Upon Each Timer Completion
    /// </summary>
    public Action OnComplete { private get; set; }

    public Timer(float timeInterval)
    {
        Time = 0;
        TimeInterval = timeInterval;
        Pause = false;
    }
    public Timer(float timeInterval, bool triggerFristFrame = false)
    {
        if(triggerFristFrame == true)
        {
            Time = timeInterval;
            TimeInterval = timeInterval;
        }

        Pause = false;
    }
    public bool TimeGreaterThan
    {
        get
        {
            if (Pause == false)
            {
                if (Time >= TimeInterval)
                {
                    Time = 0;
                    if(OnComplete != null) { OnComplete(); }
                    return true;
                }

                Time += UnityEngine.Time.deltaTime;
                return false;
            }
            return false;
        }
    }

    public bool TimerGreaterThan(out float time)
    {
        if(Pause == false)
        {
            if (Time > TimeInterval)
            {
                Time = 0;
                time = Time;
                if (OnComplete != null) { OnComplete(); }
                return true;
            }

            Time += UnityEngine.Time.deltaTime;
            time = Time;
            return false;
        }

        time = Time;
        return false;
    }

    public void SetTimeInterval(float amount)
    {
        TimeInterval = amount;
    }

    public void SetTime(float time)
    {
        Time = time;
    }

    public void PauseTimer(bool active = true)
    {
        Pause = active;
    }

    public void ResetTimer(float interval, bool isPaused = false)
    {
        Time = 0;
        TimeInterval = interval;
        Pause = isPaused;
    }
}