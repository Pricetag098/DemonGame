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
    float timer;
    float maxTimer;
    Vector3 start,mid, end,offset;
    Transform target;
    bool useTarget;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        //pooledObject = GetComponent<PooledObject>();
        visualiser = GetComponentInChildren<ProjectileVisualiser>();
    }

    public void Shoot(Vector3 origin,Vector3 mid,Vector3 end,float time,float dmg,Ability ability,int penetrations)
	{
        body.isKinematic = false;
        damage = dmg;
        transform.position = origin;
        useTarget = false;
        timer = 0;
        start = origin;
        this.mid = mid;
        this.end = end;
        maxTimer = time;
        this.ability = ability;
        visualiser.Start(ability.caster.castOrigin, transform);
        maxPenetrations = penetrations;
	}

    public void Shoot(Vector3 origin, Vector3 mid, Transform end,Vector3 offset, float time, float dmg, Ability ability, int penetrations)
    {
        body.isKinematic = false;
        damage = dmg;
        transform.position = origin;
        this.offset = offset;
        timer = 0;
        start = origin;
        this.mid = mid;
        target = end;
        useTarget = true;
        maxTimer = time;
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

	private void Update()
	{
        if (useTarget)
            end = target.position + offset;
        timer+=Time.deltaTime;
        if(timer > maxTimer)
            GetComponent<PooledObject>().Despawn();
        float t = timer / maxTimer;
        transform.position = QaudraticLerp(start,mid,end,t);
        transform.forward = Vector3.Lerp(mid - start, end - mid,t);
	}

    Vector3 QaudraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(ab, bc, t);
    }

}
