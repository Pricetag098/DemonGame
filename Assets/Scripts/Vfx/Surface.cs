using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour
{
    [SerializeField] SurfaceData data;
	public bool Penetrable = false;
    public void PlayHitVfx(Vector3 pos, Vector3 dir)
	{
		if (data.hitFx.Enabled) 
			data.hitFx.Value.Play(pos, dir);
	}

	public void PlayStepVfx(Vector3 pos, Vector3 dir)
	{
		if (data.stepFx.Enabled)
			data.stepFx.Value.Play(pos, dir);
	}
}
