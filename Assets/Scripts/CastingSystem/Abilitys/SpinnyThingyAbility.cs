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
	[SerializeField] int count;
	[SerializeField] float spread,frequncey;
	GameObject[] obj;
	float timer = -1;
	float maxTimer =1;
	float stepDirection = 1;
	Vector3 target;
	Vector3 dir;
	
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
		dir = direction;
		for (int i = 0; i < count; i++)
		{
			obj[i].SetActive(true);
			obj[i].transform.forward = direction;
		}
		
		healths.Clear();
		caster.RemoveBlood(bloodCost);

	}

	protected override void OnEquip()
	{
		obj = new GameObject[count];
		for(int i = 0; i < count; i++)
		{
			obj[i] = Instantiate(prefab);
		}
		

	}
	protected override void OnDeEquip()
	{
		for (int i = 0; i < count; i++)
		{
			Destroy(obj[i]);
		}
	}
	List<Health> healths = new List<Health>();
	public override void Tick()
	{
		if (timer < 0)
		{
			for (int i = 0; i < count; i++)
			{
				obj[i].SetActive(false);
			}
			return;
		}
		timer += Time.deltaTime * stepDirection;
		if (timer > maxTimer)
		{
			stepDirection = -1;
			healths.Clear();
		}
		float t = timer / maxTimer;
		for (int i = 0; i < count; i++)
		{
			float val = (spread * Mathf.Sin((t*Mathf.PI)*frequncey) * (i * 2 - 1));
			Debug.Log(val);
			obj[i].transform.position = Vector3.Lerp(caster.castOrigin.position, target, t) + Vector3.Cross(Vector3.up, dir) * val;
		}

		for (int i = 0; i < count; i++)
		{
			Collider[] hits = Physics.OverlapSphere(obj[i].transform.position, checkRad, enemyLayers);
			foreach (Collider hit in hits)
			{
				HitBox hb;
				if (hit.TryGetComponent(out hb))
				{
					if (!healths.Contains(hb.health))
					{
						healths.Add(hb.health);
						hb.OnHit(damage.Evaluate(timer / maxTimer));
						Vector3 point = hit.ClosestPoint(obj[i].transform.position);
						vfx.Play(point, obj[i].transform.position - point);
						OnHit(hb.health);
					}
				}
			}
		}
	}
	public override void OnHit(Health health)
	{
		if (caster.playerStats.Enabled)
			caster.playerStats.Value.GainPoints(points);
	}
}
