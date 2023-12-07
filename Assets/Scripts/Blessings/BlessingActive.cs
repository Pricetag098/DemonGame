using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BlessingActive : MonoBehaviour
{
    BlessingStatusHandler blessingStatusHandler;
    TextMeshProUGUI[] images;

    void Awake()
    {
        blessingStatusHandler = FindObjectOfType<BlessingStatusHandler>();
        images = GetComponentsInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i< images.Length;i++)
        {
            if (i < blessingStatusHandler.activeBlessings.Count)
            {
                images[i].gameObject.SetActive(true);
                images[i].text = blessingStatusHandler.activeBlessings[i].blessingFontRef;
                images[i].font = blessingStatusHandler.activeBlessings[i].blessingFontAsset;
            }
            else
            {
                images[i].gameObject.SetActive(false);
            }
        }
    }
}
