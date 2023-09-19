using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSequence : MonoBehaviour
{
    public GameObject[] settingTabs;

    public void ActivateSettings()
    {
        foreach (var tab in settingTabs)
        {
            tab.GetComponent<FadeInTween>().TweenIn();
        }
    }
}
