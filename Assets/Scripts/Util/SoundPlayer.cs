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
    public bool looping;
    public bool isPlaying = false;
    AudioSource source;
    
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        
    }
	private void OnEnable()
	{
        if (playOnAwake)
        {
            Play();
        }
    }

	// Update is called once per frame
	void Update()
    {
		if (isPlaying && !source.isPlaying)
		{
            isPlaying = false;
			if (looping)
			{
                Play();
			}
		}
		
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
        isPlaying = true;
    }

    public void PlayClip(AudioClip clip)
    {
        float rand = (Random.value - .5f) * 2;
        rand *= pitchRange;
        source.pitch = basePitch + rand;
        source.clip = clip;
        source.Play();
        isPlaying = true;
    }

    public void Stop()
	{
        isPlaying = false;
        source.Stop();
	}

}
