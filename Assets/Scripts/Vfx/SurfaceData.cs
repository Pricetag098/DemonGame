using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SurfaceData")]
public class SurfaceData : ScriptableObject
{
	public Optional<VfxSpawnRequest> hitFx;
	public Optional<VfxSpawnRequest> meleeHitFx;
	public Optional<VfxSpawnRequest> stepFx;
	public Optional<VfxSpawnRequest> slideFx;
	public Optional<VfxSpawnRequest> jumpFx;

	public float speedModifier = 1;

	public void PlayHitVfx(Vector3 pos, Vector3 dir)
	{
		if (hitFx.Enabled)
			hitFx.Value.Play(pos, dir);
	}

	public void PlayMeleeHitVfx(Vector3 pos, Vector3 dir)
	{
		if (meleeHitFx.Enabled)
			meleeHitFx.Value.Play(pos, dir);
	}

	public void PlayStepVfx(Vector3 pos, Vector3 dir)
	{
		if (stepFx.Enabled)
			stepFx.Value.Play(pos, dir);
	}
	public void PlaySlideVfx(Vector3 pos, Vector3 dir)
	{
		if (slideFx.Enabled)
			slideFx.Value.Play(pos, dir);
	}

	public void PlayJumpVfx(Vector3 pos, Vector3 dir)
	{
		if (jumpFx.Enabled)
			jumpFx.Value.Play(pos, dir);
	}
}
