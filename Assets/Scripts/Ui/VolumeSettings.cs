using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;
using static VolumeSettings;


public class VolumeSettings : MonoBehaviour,IDataPersistance<PlayerSettings>
{
    public Setting master;
    public Setting music;
    public Setting sfx;
    public Setting ambient;



    [SerializeField] AudioMixer mixerGroup;

    [Serializable]
    public class Setting
    {
        public float volume;
        public TextSlider slider;
        public string mixerParam;

        [HideInInspector] public VolumeSettings volumeSettings;
        public void SetValue(float value)
        {
            volumeSettings.SetVolume(this, value);
        }
    }
    void SetupVolume(Setting setting,float value)
    {
        
        setting.volumeSettings = this;
        setting.slider.SliderValueChanged += setting.SetValue;
        
        SetVolume(setting, value);
    }
    public void SetVolume(Setting setting,float value)
    {
        Debug.Log(value + setting.mixerParam);
        setting.volume = value;
        value = Mathf.Log10((Mathf.Max(0.001f, value) / 100)) * 20;
        mixerGroup.SetFloat(setting.mixerParam, value);
    }
    public void LoadData(PlayerSettings data)
    {
        SetupVolume(master, data.masterVolume);
        SetupVolume(music, data.musicVolume);
        SetupVolume(sfx, data.sfxVolume);
        SetupVolume(ambient, data.ambientVolume);

    }


  
    

    



    public void SaveData(ref PlayerSettings data)
    {
        data.masterVolume = master.volume;
        data.sfxVolume = sfx.volume;
        data.musicVolume = music.volume;
        data.ambientVolume = ambient.volume;

        
    }

    // Start is called before the first frame update
    void Start()
    {
        master.slider.SetValue(master.volume);
        music.slider.SetValue(music.volume);
        sfx.slider.SetValue(sfx.volume);
        ambient.slider.SetValue(ambient.volume);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
