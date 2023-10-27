using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class SliderUpdater : MonoBehaviour
{
    public TextMeshProUGUI text;

    public Slider slider;

    public float maxValue;
    public float minValue;

    public float startValue;

    private void Start()
    {
        slider.value = startValue;
    }

    private void Update()
    {
        slider.maxValue= maxValue;
        slider.minValue= minValue;

        if (slider.value <= minValue) { slider.value = minValue; }
        if (slider.value >= maxValue) { slider.value = maxValue;}

        text.text = slider.value.ToString("0");
    }
}
