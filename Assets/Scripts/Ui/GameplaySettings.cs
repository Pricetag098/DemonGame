using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplaySettings : MonoBehaviour
{
    Movement.PlayerInputt playerMovement;
    [SerializeField] Toggle toggleSlide, toggleSprint;
    [SerializeField] TextSlider sensitivity, fovSlider;
    private void Awake()
    {
         playerMovement = FindObjectOfType<Movement.PlayerInputt>();
    }
    // Start is called before the first frame update
    void Start()
    {
        sensitivity.SetValue(playerMovement.sensitivity);
        sensitivity.SliderValueChanged += SetSensitivity;

        toggleSlide.isOn = playerMovement.toggleSlide;
        toggleSprint.isOn = playerMovement.toggleSprint;
    }

    void SetSensitivity(float sensitivity)
    {
        playerMovement.sensitivity = sensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
