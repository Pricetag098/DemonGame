using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SurfaceData")]
public class SurfaceData : ScriptableObject
{
	public Optional<VfxSpawnRequest> hitFx;
	public Optional<VfxSpawnRequest> stepFx;

	public void PlayHitVfx(Vector3 pos, Vector3 dir)
	{
		if (hitFx.Enabled)
			hitFx.Value.Play(pos, dir);
	}

	public void PlayStepVfx(Vector3 pos, Vector3 dir)
	{
		if (stepFx.Enabled)
			stepFx.Value.Play(pos, dir);
	}
}
