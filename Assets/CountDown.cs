using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CountDown : MonoBehaviour
{
    public FadeInTween fade;

    public string textBeforeTimer;
    public float timeToCountFrom;
    public string textAfterTimer;

    public TextMeshProUGUI timerText;

    float timer;
    bool timerActive = false;

    private void Awake()
    {
        timer = timeToCountFrom;
    }

    private void Update()
    {
        if (timerActive)
        {
            Countdown();
        }
    }

    public void Countdown()
    {
        timer -= Time.deltaTime;

        timerText.text = textBeforeTimer + " " + timer.ToString("0") + " " + textAfterTimer;

        if (timer <= 0) { timerActive = false; OnCompleteCountdown(); }
    }

    public void OnCompleteCountdown()
    {
        //likely start game scene or trasition to loading screen here
    }

    public void CancelCountdown()
    {
        timerActive = false;
        timer = timeToCountFrom;
        fade.canvasGroup.DOKill();
        fade.canvasGroup.alpha = 0f;
    }

    public void ActivateCountdown()
    {
        timerActive = true;
    }
}
