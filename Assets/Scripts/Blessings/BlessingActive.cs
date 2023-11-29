using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlessingActive : MonoBehaviour
{
    BlessingStatusHandler blessingStatusHandler;
    Image[] images;

    private Color alphaOn = Color.white;
    private Color alphaOff = new Color(0, 0, 0, 0);
    // Start is called before the first frame update
    void Awake()
    {
        blessingStatusHandler = FindObjectOfType<BlessingStatusHandler>();
        images = GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i< images.Length;i++)
        {
            if (i < blessingStatusHandler.activeBlessings.Count)
            {
                images[i].gameObject.SetActive(true);
                images[i].color = alphaOn;
                images[i].sprite = blessingStatusHandler.activeBlessings[i].blessingImage;
            }
            else
            {
                images[i].gameObject.SetActive(false);
                images[i].color = alphaOff;
            }
        }
    }
}
