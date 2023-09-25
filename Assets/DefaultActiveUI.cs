using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultActiveUI : MonoBehaviour
{
    public GameObject[] UiPanels;
    void Start()
    {
        foreach (GameObject panel in UiPanels)
        {
            panel.SetActive(false);
        }

        UiPanels[0].SetActive(true);
    }
}
