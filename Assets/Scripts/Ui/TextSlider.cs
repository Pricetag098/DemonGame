using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSlider : MonoBehaviour
{
    [SerializeField]Slider slider;
    [SerializeField]TMP_InputField inputField;
    public delegate void SliderValueChangedDelegate(float value);
    public SliderValueChangedDelegate SliderValueChanged;

    private void Awake()
    {
        //volumeSettings = FindObjectOfType<VolumeSettings>(true);
        slider = GetComponentInChildren<Slider>(true);
        inputField = GetComponentInChildren<TMP_InputField>(true);
    }
    private void Start()
    {
        slider.onValueChanged.AddListener(SliderChange);
        inputField.onEndEdit.AddListener(FieldChange);
    }
    

    public void SliderChange(float value)
    {
        inputField.text = value.ToString("F1");
        //volumeSettings.SetVolume(setting, value);
        ChangeValue(value);
    }

    void ChangeValue(float value)
    {
        if(SliderValueChanged != null)
            SliderValueChanged(value);
    }

    public void FieldChange(string textValue)
    {
        float value = float.Parse(textValue);
        if(value < slider.minValue || value > slider.maxValue)
        {
            value = Mathf.Clamp(value, slider.minValue, slider.maxValue);
            inputField.text = value.ToString("F1");
        }
        slider.value = value;
        ChangeValue(value);
        //volumeSettings.SetVolume(setting, value);
    }
    

    public void SetValue(float value)
    {
        slider.value = value;
        inputField.text = value.ToString("F1");
    }
}
