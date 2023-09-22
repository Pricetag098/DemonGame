using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxObject : MonoBehaviour
{
	ParticleSystem[] particles;
	SoundPlayer soundPlayer;
	PooledObject pooledObject;
	private void Awake()
	{
		List<ParticleSystem> list = new List<ParticleSystem>();
		for(int i = 0; i < transform.childCount; i++)
		{
			ParticleSystem ps;
			if(transform.GetChild(i).TryGetComponent(out ps))
				list.Add(ps);
		}
		particles = list.ToArray();
		soundPlayer = GetComponentInChildren<SoundPlayer>();
		pooledObject = GetComponent<PooledObject>();
		pooledObject.OnDespawn += OnDespawn;
	}

	public void Play()
	{
		if(particles.Length > 0)
			particles[Random.Range(0, particles.Length)].Play();
		soundPlayer.Play();
	}

	void OnDespawn()
	{
		foreach(ParticleSystem p in particles)
		{
			p.Stop();
		}
	}
}
