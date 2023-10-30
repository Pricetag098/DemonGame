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
	[SerializeField] int points;
    [SerializeField] float angle;
	[SerializeField] float range;
	[SerializeField] int spikeCount;
	[SerializeField] float coolDown;
	float timer;
	[SerializeField, Range(0f, 1f)] float directionWeight;
	[SerializeField] VfxSpawnRequest spawnVfx,spike;
	
	protected override void OnEquip()
	{
		
		timer = coolDown;
	}

	public override void Cast(Vector3 origin, Vector3 direction)
	{
		if (timer < coolDown)
			return;
		timer = 0;
		List<Health> healths = new List<Health>();
		caster.RemoveBlood(bloodCost);
		spawnVfx.Play(caster.castOrigin.position,direction);
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
				SpawnSpike(hit.point, hit.normal,dir, range * randCircle.y, ref healths);
			}
        }
		caster.animator.SetTrigger("Cast");
	}
	public override void Tick()
	{
		timer += Time.deltaTime;
	}
	void SpawnSpike(Vector3 pos,Vector3 normal,Vector3 aimDir, float distance, ref List<Health> healths)
	{
		float scale = distanceScale.Evaluate(distance/range);
		
		
		Vector3 up = Vector3.Slerp(normal,aimDir,directionWeight);
		spike.Play(pos,up,Vector3.one * scale);
		Collider[] colliders = Physics.OverlapCapsule(pos, pos + normal * scale, 1,targetLayers);
		foreach(Collider collider in colliders)
		{
			HitBox hb;
			if (collider.TryGetComponent(out hb))
			{
				if (!healths.Contains(hb.health))
				{
					healths.Add(hb.health);
					hb.health.TakeDmg(distanceDamage.Evaluate(distance/range) * caster.DamageMulti);
					OnHit(hb.health);
				}
			}
		}
	}
	protected override void OnDeEquip()
	{
		Destroy(pooler.gameObject);
	}

	public override void OnHit(Health health)
	{
		if (caster.playerStats.Enabled)
			caster.playerStats.Value.GainPoints(points);
	}
}
