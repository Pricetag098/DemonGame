using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    public bool playOnAwake = false;
    public List<AudioClip> clips = new List<AudioClip>();
    public float pitchRange = 0f;
    public float basePitch = 1;
    AudioSource source;
    
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        if (playOnAwake)
        {
            Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        if(clips.Count == 0 ) { return; }
        AudioClip clip = clips[Random.Range(0, clips.Count)];
        float rand = (Random.value - .5f)*2;
        rand *= pitchRange;
        source.pitch = basePitch + rand;
        source.clip = clip;
        source.Play();
     
    }

    public void Stop()
	{
        source.Stop();
	}

}
