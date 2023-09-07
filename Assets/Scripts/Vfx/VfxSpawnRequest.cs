using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Vfx Spawn Request")]
public class VfxSpawnRequest : ScriptableObject
{
	public GameObject prefab;
	public int poolSize = 10;
	public List<AudioClip> clips = new List<AudioClip>();

	public void Play(Vector3 point, Vector3 dir)
	{
		VfxSpawner.SpawnVfx(this, point, dir, Vector3.one);
	}
	public void Play(Vector3 point, Vector3 dir,Vector3 scale)
	{
		VfxSpawner.SpawnVfx(this, point, dir,scale);
	}

	private void OnValidate()
	{
		if (prefab != null)
		{
			SoundPlayer s = prefab.GetComponentInChildren<SoundPlayer>();
			s.vfxData.Value = this;
			s.enabled = true;
		}
	}
	
}
