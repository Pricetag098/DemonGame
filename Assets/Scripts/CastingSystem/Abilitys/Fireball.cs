using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    float radius;
    float damage;
    Rigidbody body;
    [SerializeField] LayerMask validLayers;
    ProjectileVisualiser visualiser;
    [SerializeField] VfxSpawnRequest explosionVfx;
    BloodFireball ability;
    [SerializeField] float maxRadius = 10;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        visualiser = GetComponentInChildren<ProjectileVisualiser>();
    }

	private void OnCollisionEnter(Collision collision)
	{
		Collider[] cols = Physics.OverlapSphere(transform.position, radius,validLayers);
        List<Health> healths = new List<Health>();
        for(int i = 0; i < cols.Length; i++)
		{
            Collider col = cols[i];
            HitBox hb;
            if(col.TryGetComponent(out hb))
			{
				if (!healths.Contains(hb.health))
				{
                    healths.Add(hb.health);
                    hb.health.TakeDmg(damage);
                    ability.OnHit();
				}
			}
		}
        GetComponent<PooledObject>().Despawn();
        explosionVfx.Play(transform.position, transform.forward,Vector3.one * radius / maxRadius);
	}

    public void Shoot(Vector3 origin, Vector3 dir, float dmg, BloodFireball ability,float rad)
    {
        
        damage = dmg;
        transform.position = origin;
        transform.forward = dir;
        body.velocity = dir;
        this.ability = ability;
        visualiser.Start(ability.caster.castOrigin, transform);
        radius = rad;
    }
	private void OnDrawGizmos()
	{
        Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
