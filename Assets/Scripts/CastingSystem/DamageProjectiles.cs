using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DamageProjectiles : MonoBehaviour
{
    public float targetRadius;
    public LayerMask targetmask;
    public float wallRadius;
    public LayerMask wallMask;

    [HideInInspector]
    public int penetrations;
    public int maxPenetrations;
    [HideInInspector]
    public float damage;
    
    PooledObject pooledObject;
    ProjectileVisualiser visualiser;
    List<Health> healths = new List<Health>();
    [HideInInspector]
    public Ability ability;
    float timer;
    float maxTimer;
    [HideInInspector]
    public Vector3 start,mid, end,offset;
    Transform target;
    bool useTarget;

    public delegate void Action(Health health);
    public Action onPenetrate;
    // Start is called before the first frame update
    void Awake()
    {
        
        //pooledObject = GetComponent<PooledObject>();
        visualiser = GetComponentInChildren<ProjectileVisualiser>();
    }

    public void Shoot(Vector3 origin,Vector3 mid,Vector3 end,float time,float dmg,Ability ability,int penetrations)
	{

        damage = dmg;
        transform.position = origin;
        lastPos = origin;
        useTarget = false;
        timer = 0;
        start = origin;
        this.mid = mid;
        this.end = end;
        maxTimer = time;
        this.ability = ability;
        visualiser.Start(ability.caster.castOrigin, transform);
        maxPenetrations = penetrations;
        
        this.penetrations = 0;
    }

    public void Shoot(Vector3 origin, Vector3 mid, Transform end,Vector3 offset, float time, float dmg, Ability ability, int penetrations)
    {
        Debug.Log("AAA");
        damage = dmg;
        transform.position = origin;
		lastPos = origin;
		this.offset = offset;
        timer = 0;
        this.penetrations = 0;
        start = origin;
        this.mid = mid;
        target = end;
        useTarget = true;
        maxTimer = time;
        this.ability = ability;
        visualiser.Start(ability.caster.castOrigin, transform);
        maxPenetrations = penetrations;
        
    }

    
    
	
    Vector3 lastPos;
	private void Update()
	{
        if (useTarget)
            end = target.position + offset;
        timer+=Time.deltaTime;
        if(timer > maxTimer)
            GetComponent<PooledObject>().Despawn();
        float t = timer / maxTimer;
        
        transform.position = (QaudraticLerp(start,mid,end,t));
        transform.forward = Vector3.Lerp(mid - start, end - mid,t);

        Collider[] colliders = Physics.OverlapCapsule(lastPos,transform.position,targetRadius,targetmask);
        
        foreach(Collider other in colliders)
        {
			HitBox hb;
			if (other.gameObject.TryGetComponent(out hb))
			{
				if (!healths.Contains(hb.health))
				{
					hb.OnHit(damage);
					healths.Add(hb.health);
					ability.OnHit(hb.health);
					penetrations++;
					if (onPenetrate != null)
						onPenetrate(hb.health);
				}
			}
			SurfaceData data;
			Surface hs;
			if (other.gameObject.TryGetComponent(out hs))
			{
				data = hs.data;
			}
			else
			{
				data = VfxSpawner.DefaultSurfaceData;
			}
			data.PlayHitVfx(other.ClosestPoint(transform.position), -transform.forward);

			if (penetrations > maxPenetrations)
			{
				
				GetComponent<PooledObject>().Despawn();
			}
		}


        Collider[] wallColliders = Physics.OverlapCapsule(lastPos, transform.position, wallRadius, wallMask);
        if(wallColliders.Length > 0)
        {
			SurfaceData data;
			Surface hs;
			if (wallColliders[0].gameObject.TryGetComponent(out hs))
			{
				data = hs.data;
			}
			else
			{
				data = VfxSpawner.DefaultSurfaceData;
			}
			data.PlayHitVfx(wallColliders[0].ClosestPoint(transform.position), -transform.forward);
			
			Debug.Log("Dead");
			GetComponent<PooledObject>().Despawn();
		}

	}

    void LateUpdate()
    {
        lastPos = transform.position;
    }

    Vector3 QaudraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(ab, bc, t);
    }
	private void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wallRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,targetRadius);
	}

}
