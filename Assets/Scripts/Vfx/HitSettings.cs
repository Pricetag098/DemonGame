using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSettings : MonoBehaviour
{
    [SerializeField] int vfxIndex;
	public bool Penetrable = false;
    public void PlayVfx(Vector3 pos, Vector3 dir)
	{
		VfxSpawner.SpawnVfx(vfxIndex, pos, dir);
	}
}
