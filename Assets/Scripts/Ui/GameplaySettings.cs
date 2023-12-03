using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplaySettings : MonoBehaviour
{
    Movement.PlayerInputt playerMovement;
    FovSpeedChanger fovSpeedChanger;
    [SerializeField] Toggle toggleSlide, toggleSprint;
    [SerializeField] TextSlider sensitivity, fovSlider;
    private void Awake()
    {
         playerMovement = FindObjectOfType<Movement.PlayerInputt>();
        fovSpeedChanger = FindObjectOfType<FovSpeedChanger>();
    }
    // Start is called before the first frame update
    void Start()
    {
        sensitivity.SetValue(playerMovement.sensitivity);
        sensitivity.SliderValueChanged += SetSensitivity;

        fovSlider.SetValue(fovSpeedChanger.fov);
        fovSlider.SliderValueChanged += SetFov;


        toggleSlide.isOn = playerMovement.toggleSlide;
        toggleSprint.isOn = playerMovement.toggleSprint;
    }
    void SetFov(float fov)
    {
        fovSpeedChanger.fov = Mathf.Clamp(fov,60,120);
    }

    void SetSensitivity(float sensitivity)
    {
        playerMovement.sensitivity = Mathf.Max(0.001f, sensitivity);
    }

}
