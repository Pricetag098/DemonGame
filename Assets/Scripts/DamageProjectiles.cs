using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProjectiles : MonoBehaviour
{
    float damage;
    Rigidbody body;
    PooledObject pooledObject;
    ProjectileVisualiser visualiser;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        //pooledObject = GetComponent<PooledObject>();
        visualiser = GetComponentInChildren<ProjectileVisualiser>();
    }

    public void Shoot(Vector3 origin,Vector3 dir,float dmg,Transform visOrigin)
	{
        body.isKinematic = false;
        damage = dmg;
        transform.position = origin;
        transform.forward = dir;
        body.velocity = dir;
        visualiser.Start(visOrigin, transform);
        
	}

	private void OnCollisionEnter(Collision collision)
	{
        HitBox hb;
        if(collision.gameObject.TryGetComponent(out hb))
            hb.OnHit(damage);
        HitSettings hs;
        if(collision.gameObject.TryGetComponent(out hs))
		{
            hs.PlayVfx(collision.GetContact(0).point, collision.GetContact(0).normal);
		}
		else
		{
            VfxSpawner.SpawnVfx(0, collision.GetContact(0).point, collision.GetContact(0).normal);
		}
        body.isKinematic = true;
		GetComponent<PooledObject>().Despawn();
        //Debug.Log(collision.gameObject);

	}
}
