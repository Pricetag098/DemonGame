using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "abilities/BEAM")]
public class BloodBeam : Ability
{
    [SerializeField] GameObject prefab;
    [SerializeField] float damage;
    [SerializeField] float maxRange;
	[SerializeField] float radius;
	LineRenderer lineRenderer;
	[SerializeField] LayerMask enemyLayers;
	[SerializeField] LayerMask wallLayer;

	protected override void OnEquip()
	{
		lineRenderer = Instantiate(prefab).GetComponent<LineRenderer>();
	}

	public override void Cast(Vector3 origin, Vector3 direction)
	{
		float range = maxRange;
		RaycastHit wallHit;
		Vector3 end = origin + direction * maxRange;
		if(Physics.Raycast(origin, direction,out wallHit, maxRange, wallLayer))
		{
			range = Vector3.Distance(origin, wallHit.point);
			end = wallHit.point;
		}

		RaycastHit[] hits = Physics.SphereCastAll(origin, radius, direction, range, enemyLayers);
		foreach(RaycastHit hit in hits)
		{
			HitBox hb;
			if(hit.transform.TryGetComponent(out hb))
			{
				hb.OnHit(damage * Time.deltaTime);
			}
		}
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, caster.castOrigin.position);
		lineRenderer.SetPosition(1, end);
		caster.RemoveBlood(bloodCost * Time.deltaTime);
		lineRenderer.enabled = true;
	}

	public override void Tick()
	{
		lineRenderer.enabled = false;
	}
	protected override void OnDeEquip()
	{
		Destroy(lineRenderer.gameObject);
	}
}
