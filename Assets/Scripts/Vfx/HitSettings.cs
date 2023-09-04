using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSettings : MonoBehaviour
{
    [SerializeField] VfxSpawnRequest vfx;
	public bool Penetrable = false;
    public void PlayVfx(Vector3 pos, Vector3 dir)
	{
		VfxSpawner.SpawnVfx(vfx, pos, dir,Vector3.one);
	}
}
