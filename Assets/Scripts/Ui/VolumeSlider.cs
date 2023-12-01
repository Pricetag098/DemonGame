using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField]Slider slider;
    [SerializeField]TMP_InputField inputField;
    [HideInInspector]public VolumeSettings.Setting setting;
    VolumeSettings volumeSettings;

    private void Awake()
    {
        volumeSettings = FindObjectOfType<VolumeSettings>(true);
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
        inputField.text = value.ToString();
        volumeSettings.SetVolume(setting, value);
    }

    public void FieldChange(string textValue)
    {
        float value = float.Parse(textValue);
        if(value < 0f || value > 100)
        {
            value = Mathf.Clamp(value, 0f, 100f);
            inputField.text = value.ToString();
        }
        slider.value = value;
        volumeSettings.SetVolume(setting, value);
    }
    

    public void SetValue(float value)
    {
        slider.value = value;
        inputField.text = value.ToString();
    }
}
