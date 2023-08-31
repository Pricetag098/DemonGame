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
	float pointTimer;
    [SerializeField] float maxRange;
	[SerializeField] float radius;
	LineRenderer lineRenderer;
	SoundPlayer sound;
	[SerializeField] LayerMask enemyLayers;
	[SerializeField] LayerMask wallLayer;

	bool held,startedCasting;
	protected override void OnEquip()
	{
		lineRenderer = Instantiate(prefab).GetComponent<LineRenderer>();
		sound = lineRenderer.GetComponent<SoundPlayer>();
	}

	public override void Cast(Vector3 origin, Vector3 direction)
	{
		if (!startedCasting)
		{
			lineRenderer.enabled = true;
			startedCasting = true;
			sound.Play();
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
						if (caster.playerStats.Enabled)
							caster.playerStats.Value.GainPoints(points);
					}
					hb.OnHit(damage * Time.deltaTime);
					healths.Add(hb.health);
				}

			}
		}
		lineRenderer.transform.position = caster.castOrigin.position;
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, caster.castOrigin.position);
		lineRenderer.SetPosition(1, end);
		caster.RemoveBlood(bloodCost * Time.deltaTime);
		

		held = true;
	}

	public override void Tick()
	{
		if (!held)
		{
			sound.Stop();
			startedCasting = false;
			lineRenderer.enabled = false;
		}
		held = false;
	}
	protected override void OnDeEquip()
	{
		Destroy(lineRenderer.gameObject);
	}
}
