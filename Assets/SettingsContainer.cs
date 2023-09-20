using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class SettingsContainer : MonoBehaviour
{
    public Transform gameplayParent;
    public Transform audioParent;
    public Transform videoParent;
    public Transform controlsParent;

    [Header("Lists")]
    public List<Transform> gameplaySettings = new List<Transform>();
    public List<Transform> audioSettings = new List<Transform>();
    public List<Transform> videoSettings = new List<Transform>();
    public List<Transform> controlSettings = new List<Transform>();

    private void Awake()
    {
        foreach(Transform child in gameplayParent)
        {
            gameplaySettings.Add(child);
        }
        foreach (Transform child in audioParent)
        {
            audioSettings.Add(child);
        }
        foreach (Transform child in videoParent)
        {
            videoSettings.Add(child);
        }
        foreach (Transform child in controlsParent)
        {
            controlSettings.Add(child);
        }
    }

    public void ActiveList(int value)
    {
        var sequence = DOTween.Sequence();

        switch (value)
        {
            case 0:
                {
                    foreach (Transform item in gameplaySettings)
                    {
                        item.GetComponent<CanvasGroup>().alpha = 0f;
                        sequence.Append(item.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
                    }
                    break;
                }
            case 1:
                {
                    foreach (Transform item in audioSettings)
                    {
                        item.GetComponent<CanvasGroup>().alpha = 0f;
                        sequence.Append(item.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
                    }
                    break;
                }
            case 2:
                {
                    foreach (Transform item in videoSettings)
                    {
                        item.GetComponent<CanvasGroup>().alpha = 0f;
                        sequence.Append(item.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
                    }
                    break;
                }
            case 3:
                {
                    foreach (Transform item in controlSettings)
                    {
                        item.GetComponent<CanvasGroup>().alpha = 0f;
                        sequence.Append(item.GetComponent<CanvasGroup>().DOFade(1, 0.1f));
                    }
                    break;
                }
        }
    }
}
