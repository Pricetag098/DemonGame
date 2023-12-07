using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplaySettings : MonoBehaviour, IDataPersistance<PlayerSettings>
{
    Movement.PlayerInputt playerMovement;
    FovSpeedChanger fovSpeedChanger;
    [SerializeField] Toggle toggleSlideUi, toggleSprintUi;
    [SerializeField] TextSlider sensitivitySlider, fovSlider;
    public float sensitivity;
    public float fov;
    public bool toggleSlide;
    public bool toggleSprint;

    public bool inGame;

    private void Awake()
    {
         playerMovement = FindObjectOfType<Movement.PlayerInputt>();
        fovSpeedChanger = FindObjectOfType<FovSpeedChanger>();
        inGame = playerMovement != null;
    }
    // Start is called before the first frame update
    void Start()
    {

        sensitivitySlider.SetValue(sensitivity);
        sensitivitySlider.SliderValueChanged += SetSensitivity;

        fovSlider.SetValue(fov);
        fovSlider.SliderValueChanged += SetFov;


        toggleSlideUi.isOn = toggleSlide;
        toggleSlideUi.onValueChanged.AddListener(SetToggleSlide);

        toggleSprintUi.isOn = toggleSprint;
        toggleSprintUi.onValueChanged.AddListener(SetToggleSprint);

        SetSensitivity(sensitivity);
        SetFov(fov);
        SetToggleSlide(toggleSlide);
        SetToggleSprint(toggleSprint);
    }
    void SetFov(float fov)
    {
        this.fov = fov;
        if(inGame)
        fovSpeedChanger.fov = Mathf.Clamp(fov,60,120);

    }

    void SetSensitivity(float sensitivity)
    {
        this.sensitivity = sensitivity;
        if(inGame)
        playerMovement.sensitivity = Mathf.Max(0.001f, sensitivity);
    }

    void SetToggleSlide(bool toggleSlide)
    {
        this.toggleSlide = toggleSlide;
        if(inGame)
            playerMovement.toggleSlide = toggleSlide;
    }

    void SetToggleSprint(bool toggleSprint)
    {
        this.toggleSprint = toggleSprint;
        if (inGame)
            playerMovement.toggleSprint = toggleSprint;
    }

    void IDataPersistance<PlayerSettings>.LoadData(PlayerSettings data)
    {
        sensitivity = data.sensitivity;
        toggleSlide = data.toggleSlide;
        toggleSprint = data.toggleSprint;
        fov = data.fov;
    }

    void IDataPersistance<PlayerSettings>.SaveData(ref PlayerSettings data)
    {
        data.sensitivity = sensitivity;
        data.toggleSlide = toggleSlide;
        data.toggleSprint = toggleSprint;
        data.fov = fov;
    }

}
