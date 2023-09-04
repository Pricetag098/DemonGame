using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "abilities/spin")]
public class SpinnyThingyAbility : Ability
{
    [SerializeField] GameObject prefab;
    [SerializeField] float maxRange;
    [SerializeField] LayerMask wallLayers;
	[SerializeField] LayerMask enemyLayers;
	[SerializeField] float checkRad;
	[SerializeField] AnimationCurve damage;
	[SerializeField] float speed;
	[SerializeField] VfxSpawnRequest vfx;
	[SerializeField] VfxSpawnRequest vfxSpawn;
	[SerializeField] int points;
	GameObject obj;
	float timer = -1;
	float maxTimer =1;
	float stepDirection = 1;
	Vector3 target;

	
	public override void Cast(Vector3 origin, Vector3 direction)
	{
		if (timer > 0)
			return;
		vfxSpawn.Play(caster.castOrigin.position, direction);
		RaycastHit hit;
		if(Physics.Raycast(origin, direction, out hit, maxRange, wallLayers))
		{
			target = hit.point;
		}
		else
		{
			target = origin + direction * maxRange;
		}
		timer = 0;
		maxTimer = Vector3.Distance(origin, target) / speed;
		stepDirection = 1f;
		obj.SetActive(true);
		obj.transform.forward = direction;
		healths.Clear();
		caster.RemoveBlood(bloodCost);

	}

	protected override void OnEquip()
	{
		obj = Instantiate(prefab);

	}
	protected override void OnDeEquip()
	{
		Destroy(obj);
	}
	List<Health> healths = new List<Health>();
	public override void Tick()
	{
		if (timer < 0)
		{
			obj.SetActive(false);
			return;
		}
		timer += Time.deltaTime * stepDirection;
		if (timer > maxTimer)
		{
			stepDirection = -1;
			healths.Clear();
		}
		obj.transform.position = Vector3.Lerp(caster.castOrigin.position, target, timer/maxTimer);

		Collider[] hits = Physics.OverlapSphere(obj.transform.position, checkRad, enemyLayers);
		foreach (Collider hit in hits)
		{
			HitBox hb;
			if (hit.TryGetComponent(out hb))
			{
				if (!healths.Contains(hb.health))
				{
					healths.Add(hb.health);
					hb.OnHit(damage.Evaluate(timer / maxTimer));
					Vector3 point = hit.ClosestPoint(obj.transform.position);
					vfx.Play(point, obj.transform.position - point);
					OnHit();
				}
			}
		}

	}
	public override void OnHit()
	{
		if (caster.playerStats.Enabled)
			caster.playerStats.Value.GainPoints(points);
	}
}
