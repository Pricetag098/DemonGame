using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour,IDataPersistance<PlayerSettings>
{
    public float masterVolume = 0;
    public float sfxvolume = 0;
    public float musicVolume = 0;
    public float ambientVolume = 0;

    [SerializeField] Slider masterSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider ambientSlider;

    [SerializeField] AudioMixerGroup mixerGroup;

    public void LoadData(PlayerSettings data)
    {
        masterVolume = data.masterVolume;
        sfxvolume = data.sfxvolume;
        musicVolume = data.musicVolume;
        ambientVolume = data.ambientVolume;

        masterSlider.value = masterVolume;
        sfxSlider.value = sfxvolume;
        musicSlider.value = musicVolume;
        ambientSlider.value = ambientVolume;


    }

    public void SaveData(ref PlayerSettings data)
    {
        data.masterVolume = masterVolume;
        data.sfxvolume = sfxvolume;
        data.musicVolume = musicVolume;
        data.ambientVolume = ambientVolume;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
