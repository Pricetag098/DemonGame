using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CountDown : MonoBehaviour
{
    public FadeInTween fade;

    public LoadingBar loadingBar;

    public string textBeforeTimer;
    public float timeToCountFrom;
    public string textAfterTimer;

    public TextMeshProUGUI timerText;

    float timer;
    bool timerActive = false;

    //private Timer timer;

    public GameObject[] panels;
    public GameObject[] buttons;

    public SoundPlayer enterSFX = null;

    private void Awake()
    {
        //timer = new Timer()
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
        enterSFX.Play();
        panels[0].SetActive(false);
        panels[1].SetActive(true);

        buttons[0].SetActive(false);
        buttons[1].SetActive(true);

        //loadingBar.isLoading = true;
        timer = timeToCountFrom;
        fade.canvasGroup.alpha = 0f;
        loadingBar.StartLoading();
        
        //likely start game scene or trasition to loading screen here
    }

    public void CancelCountdown()
    {
        buttons[0].SetActive(false);
        buttons[1].SetActive(true);
        timerActive = false;
        timer = timeToCountFrom;
        fade.canvasGroup.DOKill();
        fade.canvasGroup.alpha = 0f;
    }

    public void ActivateCountdown()
    {
        timerActive = true;

        buttons[0].SetActive(true);
        buttons[1].SetActive(false);
    }
}
