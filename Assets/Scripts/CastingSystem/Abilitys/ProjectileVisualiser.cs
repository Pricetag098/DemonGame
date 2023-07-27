using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileVisualiser : MonoBehaviour
{
     Transform origin;
     Transform target;
    float timer;
    public float moveSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Start(Transform start, Transform end)
	{
        timer = 0;
        origin = start;
        target = end;
	}

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * moveSpeed;
        transform.position = Vector3.Lerp(origin.position,target.position,timer);
        transform.forward = target.forward;
    }
}
