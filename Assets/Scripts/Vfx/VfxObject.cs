using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxObject : MonoBehaviour
{
	Optional<ParticleSystem> particle;
	Optional<SoundPlayer> soundPlayer;
	PooledObject pooledObject;
	private void Awake()
	{
		particle.Value = GetComponentInChildren<ParticleSystem>();
		soundPlayer.Value = GetComponentInChildren<SoundPlayer>();
		particle.Enabled = particle.Value != null;
		soundPlayer.Enabled = soundPlayer.Value != null;

		pooledObject = GetComponent<PooledObject>();
		pooledObject.OnDespawn += OnDespawn;
	}

	public void Play()
	{
		if (particle.Enabled)
			particle.Value.Play();
		if (soundPlayer.Enabled)
			soundPlayer.Value.Play();
	}

	void OnDespawn()
	{
		if (particle.Enabled)
			particle.Value.Play();
		if (soundPlayer.Enabled)
			soundPlayer.Value.Play();
	}
}
