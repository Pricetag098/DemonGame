using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public GameObject[] panels;

    public Slider slider;

    [TextArea(2,5)]
    public string toolTipText;

    public TextMeshProUGUI textObject;

    public bool isLoading = false;
    private void Awake()
    {
        textObject.text = toolTipText;
        slider.value= 0;
        slider.maxValue = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoading) { Loading(); }

        if (slider.value >= slider.maxValue) { isLoading = false; LoadScene(); }
    }

    void LoadScene()
    {
        Debug.Log("the game was loaded");

        panels[0].SetActive(false);
        panels[1].SetActive(true);

        slider.value = 0;
        //will load the next scene
    }

    public void Loading()
    {
        slider.value += Time.deltaTime;
    }
}
