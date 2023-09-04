using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProjectiles : MonoBehaviour
{
    public int maxPenetrations;
    float damage;
    Rigidbody body;
    PooledObject pooledObject;
    ProjectileVisualiser visualiser;
    List<Health> healths = new List<Health>();
    Ability ability;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        //pooledObject = GetComponent<PooledObject>();
        visualiser = GetComponentInChildren<ProjectileVisualiser>();
    }

    public void Shoot(Vector3 origin,Vector3 dir,float dmg,Ability ability,int penetrations)
	{
        body.isKinematic = false;
        damage = dmg;
        transform.position = origin;
        transform.forward = dir;
        body.velocity = dir;
        this.ability = ability;
        visualiser.Start(ability.caster.castOrigin, transform);
        maxPenetrations = penetrations;
	}
    int penetrations;
	private void OnTriggerEnter(Collider other)
	{
        
        HitBox hb;
        if(other.gameObject.TryGetComponent(out hb))
        {
            if (!healths.Contains(hb.health))
            {
                hb.OnHit(damage);
                healths.Add(hb.health);
                ability.OnHit();
            }
        }
            
        HitSettings hs;
        if(other.gameObject.TryGetComponent(out hs))
		{
            hs.PlayVfx(other.ClosestPoint(transform.position), -transform.forward);
		}
		else
		{
            VfxSpawner.SpawnVfx(0, other.ClosestPoint(transform.position), -transform.forward,Vector3.one);
		}
        
        if(penetrations > maxPenetrations)
        {
            body.isKinematic = true;
            GetComponent<PooledObject>().Despawn();
        }
		
        //Debug.Log(collision.gameObject);

	}
    private void OnCollisionEnter(Collision collision)
    {
        HitSettings hs;
        if (collision.gameObject.TryGetComponent(out hs))
        {
            hs.PlayVfx(collision.collider.ClosestPoint(transform.position), -transform.forward);
        }
        else
        {

            VfxSpawner.SpawnVfx(0, collision.collider.ClosestPoint(transform.position), -transform.forward,Vector3.one);
        }
        body.isKinematic = true;
        GetComponent<PooledObject>().Despawn();
    }
}
