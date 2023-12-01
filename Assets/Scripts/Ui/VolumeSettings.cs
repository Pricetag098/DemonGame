using System;
using UnityEngine;
using UnityEngine.Audio;


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
        public VolumeSlider slider;
        public string mixerParam;
    }
    void SetupVolume(Setting setting,float value)
    {
        Debug.Log(value);
        setting.slider.setting = setting;
        setting.slider.SetValue(value);
        SetVolume(setting,value);
    }
    public void SetVolume(Setting setting,float value)
    {
        setting.volume = value;
        value = Mathf.Log10((Mathf.Max(0.001f, value) / 100)) * 20;
        mixerGroup.SetFloat(setting.mixerParam, value);
    }
    public void LoadData(PlayerSettings data)
    {
        Debug.Log("Test");
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
