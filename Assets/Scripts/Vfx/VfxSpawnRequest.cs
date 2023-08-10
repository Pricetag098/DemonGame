using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Vfx Spawn Request")]
public class VfxSpawnRequest : ScriptableObject
{
	public GameObject prefab;
	public int poolSize = 10;

	public void Play(Vector3 point, Vector3 dir)
	{
		VfxSpawner.SpawnVfx(this, point, dir);
	}
}
