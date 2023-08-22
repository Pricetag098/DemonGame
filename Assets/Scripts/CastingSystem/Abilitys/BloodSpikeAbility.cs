using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "abilities/spikes")]
public class BloodSpikeAbility : Ability
{
	[SerializeField] GameObject prefab;
	ObjectPooler pooler;
	[SerializeField] int poolSize = 100;
	[SerializeField] LayerMask targetLayers;
	[SerializeField] LayerMask groundLayers;
	[SerializeField] AnimationCurve distanceScale;
    [SerializeField] AnimationCurve distanceDamage;
    [SerializeField] float angle;
	[SerializeField] float range;
	[SerializeField] int spikeCount;
	
	protected override void OnEquip()
	{
		pooler = new GameObject().AddComponent<ObjectPooler>();
		pooler.CreatePool(prefab, 100);
	}

	public override void Cast(Vector3 origin, Vector3 direction)
	{
		List<Health> healths = new List<Health>();
		caster.RemoveBlood(bloodCost);

		for(int i = 0; i < spikeCount; i++)
		{
			Vector3 aimdir = direction;
            aimdir.y = 0;
			aimdir.Normalize();
            Vector2 randCircle = Random.insideUnitCircle;
			randCircle.y = Mathf.Abs(randCircle.y);
            
            Vector3 dir = Quaternion.Euler(0, randCircle.x * angle, 0) * aimdir;
            Vector3 point = origin + dir * randCircle.y * range;
			RaycastHit hit;
			if(Physics.Raycast(point,Vector3.down,out hit, 10, groundLayers))
			{
				SpawnSpike(hit.point, hit.normal, range * randCircle.y, ref healths);
			}
        }
	}

	void SpawnSpike(Vector3 pos,Vector3 normal, float distance, ref List<Health> healths)
	{
		float scale = distanceScale.Evaluate(distance/range);
		GameObject spike = pooler.Spawn();
		spike.transform.position = pos;
		spike.transform.up = normal;
		spike.transform.localScale = Vector3.one * scale;
		Collider[] colliders = Physics.OverlapCapsule(pos, pos + normal * scale, .1f * scale,targetLayers);
		foreach(Collider collider in colliders)
		{
			HitBox hb;
			if (collider.TryGetComponent(out hb))
			{
				if (!healths.Contains(hb.health))
				{
					healths.Add(hb.health);
					hb.health.TakeDmg(distanceDamage.Evaluate(distance/range));
				}
			}
		}
	}
	protected override void OnDeEquip()
	{
		Destroy(pooler.gameObject);
	}


}
