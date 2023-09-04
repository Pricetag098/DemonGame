using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "abilities/immolate")]
public class ImmolateAbility : Ability
{

	[SerializeField] int maxDepth;
	int curentDepth = 0;
	[SerializeField] float checkRad;
	[SerializeField] float range;
	[SerializeField] float damage;
	[SerializeField] VfxSpawnRequest vfx;
	[SerializeField] LayerMask enemyLayer;
	[SerializeField] LayerMask rayLayer;
	List<Health> healths = new List<Health>();

	List<Health> targets = new List<Health>();

	List<Particle> particles = new List<Particle>();
	ObjectPooler pooler;
	[SerializeField] GameObject prefab;

	float timer;
	[SerializeField] float timeBetweenHits;

	bool casted = false;

	struct Particle
	{
		public Vector3 origin;
		public Transform target;
		public GameObject obj;

		public Particle(Vector3 origin,Transform target,GameObject obj)
		{
			this.origin = origin;
			this.target = target;
			this.obj = obj;
			obj.transform.position = origin;
			obj.GetComponent<ParticleSystem>().Play();
		}
	}
	public override void Tick()
	{
		if (casted)
		{
			timer += Time.deltaTime;
			float t = timer / timeBetweenHits;
			foreach(Particle particle in particles)
			{
				particle.obj.transform.position = Vector3.Lerp(particle.origin,particle.target.position,t);
			}
			if(timer > timeBetweenHits)
			{
				timer = 0;

				foreach (Particle particle in particles)
				{
					particle.obj.GetComponent<ParticleSystem>().Stop();
					particle.obj.GetComponent<PooledObject>().Despawn();
				}
				particles.Clear();

				bool lastHits = curentDepth >= maxDepth;
				Debug.Log(lastHits);
				List<Health> newTargets = new List<Health>();
				foreach(Health health in targets)
				{
					Debug.Log(health);
					HitTarget(health);
					if (!lastHits) 
						AddTargets(health,ref newTargets);
				}
				targets = newTargets;
				if (lastHits || newTargets.Count == 0)
				{
					casted = false;
				}

				curentDepth++;

			}
		}
	}
	public override void Cast(Vector3 origin, Vector3 direction)
	{
		if (casted)
			return;
		healths.Clear();
		targets.Clear();
		curentDepth = 0;
		foreach(Particle particle in particles)
		{
			particle.obj.GetComponent<ParticleSystem>().Stop();
			particle.obj.GetComponent<PooledObject>().Despawn();
		}
		particles.Clear();
		RaycastHit hit;
		if(Physics.Raycast(origin, direction, out hit, range, rayLayer))
		{
			
			HitBox hb;
			if(hit.collider.TryGetComponent(out hb))
			{
				Debug.Log(hb);
				healths.Add(hb.health);
				targets.Add(hb.health);
				casted = true;
				timer = 0;
				caster.RemoveBlood(bloodCost);
				if (hb.health.vfxTarget.Enabled)
				{
					particles.Add(new Particle(caster.castOrigin.position, hb.health.vfxTarget.Value.core, pooler.Spawn()));
				}
			}
		}
	}

	protected override void OnEquip()
	{
		pooler = new GameObject().AddComponent<ObjectPooler>();
		pooler.CreatePool(prefab, 30);
	}

	public void HitTarget(Health health)
	{
		health.TakeDmg(damage);
		vfx.Play(health.GetComponent<VfxTargets>().origin.position,Vector3.up);
	}

	public void AddTargets(Health origin,ref List<Health> newTargets)
	{
		Collider[] hits = Physics.OverlapSphere(origin.transform.position, checkRad, enemyLayer);
		foreach (Collider hit in hits)
		{
			HitBox hb;
			if(hit.TryGetComponent(out hb))
			{
				if (!healths.Contains(hb.health))
				{
					healths.Add(hb.health);
					newTargets.Add(hb.health);
					
					if(hb.health.vfxTarget.Enabled && origin.vfxTarget.Enabled)
					{
						Particle particle = new Particle(origin.vfxTarget.Value.core.position,
						hb.health.vfxTarget.Value.core,
						pooler.Spawn());
						particles.Add(particle);
					}
				}
			}
		}
	}
}
