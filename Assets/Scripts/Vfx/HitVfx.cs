using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitVfx : MonoBehaviour
{
    [SerializeField] int vfxIndex;

    public void Play(Vector3 pos, Vector3 dir)
	{
		VfxSpawner.SpawnVfx(vfxIndex, pos, dir);
	}
}
