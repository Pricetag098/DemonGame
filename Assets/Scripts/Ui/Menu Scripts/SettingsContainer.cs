using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class SettingsContainer : MonoBehaviour
{
    public bool[] clickBools = new bool[] {false, false, false, false};

    public GameObject[] pages;
    public Transform[] parents;

    [Header("Lists")]
    public List<Transform> gameplaySettings = new List<Transform>();
    public List<Transform> audioSettings = new List<Transform>();
    public List<Transform> videoSettings = new List<Transform>();
    public List<Transform> controlSettings = new List<Transform>();

    private void Awake()
    {
        foreach(Transform child in parents[0])
        {
            gameplaySettings.Add(child);
        }
        foreach (Transform child in parents[1])
        {
            audioSettings.Add(child);
        }
        foreach (Transform child in parents[2])
        {
            videoSettings.Add(child);
        }
        foreach (Transform child in parents[3])
        {
            controlSettings.Add(child);
        }
    }

    public void ActiveList(int value)
    {
        switch (value)
        {
            case 0:
                {
                    ActivePage(value);
                    PlayList(gameplaySettings, value);
                    break;
                }
            case 1:
                {
                    ActivePage(value);
                    PlayList(audioSettings, value);
                    break;
                }
            case 2:
                {
                    ActivePage(value);
                    PlayList(videoSettings, value);
                    break;
                }
            case 3:
                {
                    ActivePage(value);
                    PlayList(controlSettings, value);
                    break;
                }
        }
    }

    public void CloseSettingsMenu()
    {
        for (int i = 0; i < clickBools.Length; i++)
        {
            clickBools[i] = false;
        }
    }

    public void PlayList(List<Transform> list, int pageBool)
    {
        var sequence = DOTween.Sequence();

        if (!clickBools[pageBool])
        {
            clickBools[pageBool] = true;
            foreach (Transform item in list)
            {
                item.GetComponent<CanvasGroup>().alpha = 0f;
                sequence.Append(item.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
            }
        }

    }

    public void ActivePage(int activePage)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }
        pages[activePage].SetActive(true);
    }
}
