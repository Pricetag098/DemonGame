using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    bool isTracking;

    float time;

    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] TextMeshProUGUI deathText;

    private void Start()
    {
        isTracking = true;
        time = 0;
    }

    private void Update()
    {
        if (isTracking)
        {
            time += Time.deltaTime;
        }
    }

    public void DisplayTime(bool dead)
    {
        isTracking = false;

        TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        string displayText = string.Format("{0:00}:{1:00}:{2:00}:{3:00}",
                                           timeSpan.Hours,
                                           timeSpan.Minutes,
                                           timeSpan.Seconds,
                                           timeSpan.Milliseconds / 10);

        if (dead)
        {
            deathText.text = displayText;
        }
        else
        {
            timerText.text = displayText;
        }
    }

    public void ContinueTracking()
    {
        isTracking = true;
    }
}
