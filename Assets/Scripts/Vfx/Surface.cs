using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    [SerializeField] SurfaceData data;
	public bool Penetrable = false;
    public void PlayHitVfx(Vector3 pos, Vector3 dir)
	{
		data.hitFx.Play(pos, dir);
	}

	public void PlayStepVfx(Vector3 pos, Vector3 dir)
	{
		data.stepFx.Play(pos, dir);
	}
}
