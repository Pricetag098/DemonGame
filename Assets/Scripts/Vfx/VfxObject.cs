using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxObject : MonoBehaviour
{
    ParticleSystem particle;
    SoundPlayer soundPlayer;
	PooledObject pooledObject;
	private void Awake()
	{
		particle = GetComponentInChildren<ParticleSystem>();
		soundPlayer = GetComponentInChildren<SoundPlayer>();
		pooledObject = GetComponent<PooledObject>();
		pooledObject.OnDespawn += OnDespawn;
	}

	public void Play()
	{
		particle.Play();
		soundPlayer.Play();
	}

	void OnDespawn()
	{
		particle.Stop();
		soundPlayer.Stop();
	}

	

	
}
