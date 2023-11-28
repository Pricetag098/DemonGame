using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "abilities/BEAM")]
public class BloodBeam : Ability
{
    [SerializeField] GameObject prefab;
    [SerializeField] float damage;
	[SerializeField] int points;
	[SerializeField] float pointFrequency;
	[SerializeField] float vfxWaitTime;
	[SerializeField] VfxSpawnRequest endRequest;
	float pointTimer;
    [SerializeField] float maxRange;
	[SerializeField] float radius;
	LineRenderer[] lineRenderers;
	SoundPlayer sound;
	[SerializeField] LayerMask enemyLayers;
	[SerializeField] LayerMask wallLayer;

	bool held,startedCasting;
	GameObject endVFX;

	protected override void OnEquip()
	{
		GameObject go = Instantiate(prefab);
		sound = go.GetComponent<SoundPlayer>();
		lineRenderers = go.GetComponentsInChildren<LineRenderer>();
	}

	public override void Cast(Vector3 origin, Vector3 direction)
	{
		if (!startedCasting)
        {
            Sequence wait = DOTween.Sequence();
			wait.AppendInterval(vfxWaitTime);
			wait.AppendCallback(() => EnableLineRenderer(true));
			wait.AppendCallback(() => endVFX = endRequest.PlayReturn(Vector3.zero, caster.transform.forward, Vector3.one).gameObject);
            startedCasting = true;
            sound.Play();
            caster.animator.SetTrigger("Cast");
            caster.animator.SetBool("Held", true);
        }

        float range = maxRange;
		pointTimer += Time.deltaTime;
		RaycastHit wallHit;
		Vector3 end = origin + direction * maxRange;
		if(Physics.Raycast(origin, direction,out wallHit, maxRange, wallLayer))
		{
			range = Vector3.Distance(origin, wallHit.point);
			end = wallHit.point;
		}
		List<Health> healths = new List<Health>();
		RaycastHit[] hits = Physics.SphereCastAll(origin, radius, direction, range, enemyLayers);
		bool getPoints = false;
		if (pointTimer > pointFrequency)
		{
			getPoints = true;
			pointTimer = 0;
		}
		foreach (RaycastHit hit in hits)
		{
			HitBox hb;
			if(hit.collider.TryGetComponent(out hb))
			{
				if (!healths.Contains(hb.health))
				{
					if (getPoints)
					{
						if(!hb.health.dead)
						{
                            if (caster.playerStats.Enabled)
                                caster.playerStats.Value.GainPoints(points);
                        }
						
					}
					hb.OnHit(damage * Time.deltaTime * caster.DamageMulti, HitType.ABILITY);
					healths.Add(hb.health);
				}

			}
		}

        foreach (LineRenderer lr in lineRenderers)
        {
            lr.transform.position = caster.castOrigin.position;
            lr.positionCount = 2;
            lr.SetPosition(0, caster.castOrigin.position);
            lr.SetPosition(1, end);
			lr.material.SetFloat("_BeamSize", range);
			if (endVFX)
			{
                endVFX.transform.position = end;
                endVFX.transform.LookAt(caster.transform);
            }
        }

        caster.RemoveBlood(bloodCost * Time.deltaTime);
		

		held = true;
	}

    private void EnableLineRenderer(bool enabled)
    {
        foreach (LineRenderer lr in lineRenderers)
        {
            lr.enabled = enabled;
        }
    }

    public override void Tick(Vector3 origin, Vector3 direction)
	{
		if (!held)
		{
			if (startedCasting)
			{
				caster.animator.SetBool("Held", false);
                endVFX.GetComponent<PooledObject>().Despawn();
            }
            sound.Stop();
			startedCasting = false;
			EnableLineRenderer(false);
        }
		held = false;
	}
	protected override void OnDeEquip()
	{
		Destroy(sound.gameObject);
	}
}
