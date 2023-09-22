using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletVisualiser : MonoBehaviour
{
    float travelTime;
    float timer;
    Vector3 origin;
    Vector3 target;
    PooledObject pooledObject;
    MeshRenderer mr;
    
    
	private void Awake()
	{
		pooledObject = GetComponent<PooledObject>();
        mr = GetComponent<MeshRenderer>();
        
	}

	// Update is called once per frame
	void Update()
    {
        
        transform.position = Vector3.Lerp(origin,target,timer/travelTime);
        if(timer >= travelTime)
		{
            //pooledObject.Despawn();
            mr.enabled = false;
            enabled = false;
		}
        timer += Time.deltaTime ;
    }

    public void Shoot(Vector3 from, Vector3 to, float time,Vector3 direction)
	{
        
        origin = from;
        target = to;
        travelTime = time;
        transform.position = origin;
        timer = 0;
        mr.enabled = true;
        enabled = true;
        transform.forward = direction;
        
    }
}
