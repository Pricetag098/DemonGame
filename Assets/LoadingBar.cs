using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public GameObject onPanel;
    public GameObject offpanel;

    Slider slider;

    public bool isLoading = false;
    private void Awake()
    {
        slider= GetComponent<Slider>();
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

        onPanel.SetActive(true);
        offpanel.SetActive(false);

        //will load the next scene
    }

    public void Loading()
    {
        slider.value += Time.deltaTime;
    }
}
