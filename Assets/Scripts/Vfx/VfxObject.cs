using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VfxObject : MonoBehaviour
{
	ParticleSystem[] particles;
	VisualEffect[] effects;
	SoundPlayer soundPlayer;
	PooledObject pooledObject;
	private void Awake()
	{
		List<ParticleSystem> list = new List<ParticleSystem>();
		List<VisualEffect> list2 = new List<VisualEffect>();
		for (int i = 0; i < transform.childCount; i++)
		{
			ParticleSystem ps;
			if(transform.GetChild(i).TryGetComponent(out ps))
				list.Add(ps);
			if(transform.GetChild(i).TryGetComponent(out VisualEffect vs))
				list2.Add(vs);

		}
		particles = list.ToArray();
		effects = list2.ToArray();
		soundPlayer = GetComponentInChildren<SoundPlayer>();
		pooledObject = GetComponent<PooledObject>();
		pooledObject.OnDespawn += OnDespawn;
	}

	public void Play()
	{
		if(particles.Length > 0)
			particles[Random.Range(0, particles.Length)].Play();

		if(effects.Length > 0)
			effects[Random.Range(0, effects.Length)].Play();
		soundPlayer.Play();
	}

	void OnDespawn()
	{
		foreach(ParticleSystem p in particles)
		{
			p.Stop();
		}
		foreach(VisualEffect v in effects)
		{ v.Stop(); }

		soundPlayer.Stop();
	}
}
