using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProjectiles : MonoBehaviour
{
    float damage;
    Rigidbody body;
    PooledObject pooledObject;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        //pooledObject = GetComponent<PooledObject>();
    }

    public void Shoot(Vector3 origin,Vector3 dir,float dmg)
	{
        damage = dmg;
        transform.position = origin;
        transform.forward = dir;
        body.velocity = dir;
	}

	private void OnCollisionEnter(Collision collision)
	{
        HitBox hb;
        if(collision.gameObject.TryGetComponent(out hb))
            hb.OnHit(damage);
		GetComponent<PooledObject>().Despawn();

	}
}
