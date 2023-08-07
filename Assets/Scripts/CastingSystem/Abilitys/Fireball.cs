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
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
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
				}
			}
		}
        body.isKinematic = true;
	}

    public void Shoot(Vector3 origin, Vector3 dir, float dmg, Transform visOrigin,float rad)
    {
        Debug.Log("Test");
        body.isKinematic = false;
        damage = dmg;
        transform.position = origin;
        transform.forward = dir;
        body.velocity = dir;
        visualiser.Start(visOrigin, transform);
        radius = rad;
    }
}
