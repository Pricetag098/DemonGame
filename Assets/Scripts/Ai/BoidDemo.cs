
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidDemo : MonoBehaviour
{
    [SerializeField] BoidDemo[] boids;
    [SerializeField] Transform target;
    
    [SerializeField] float speed =1;
    public Vector3 velocity;
    [SerializeField] float detectionRange;
    [SerializeField] float spreadSteerForce;
    [SerializeField] float spreadDetectionRange;
    [SerializeField] float groupSteerForce;
    [SerializeField] float alignSteerForce;
    [SerializeField] float targetingForce;
    // Start is called before the first frame update
    void Start()
    {
        boids = FindObjectsOfType<BoidDemo>();
        velocity =Quaternion.Euler(0,Random.value * 360,0)  *  Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 targetDirection = target.position - transform.position;
        targetDirection = targetDirection.normalized * targetingForce * Time.deltaTime;
        velocity += targetDirection;

        Vector3 closeDir = Vector3.zero;
        Vector3 avgPos = Vector3.zero;
        Vector3 avgDir = Vector3.zero;
        int neighbours = 0;
        foreach(BoidDemo boid in boids)
        {
            if (DistanceLessThan(transform.position, boid.transform.position, spreadDetectionRange))
            {
                closeDir += transform.position - boid.transform.position;
            }
            else if(DistanceLessThan(transform.position, boid.transform.position, detectionRange))
            {
                neighbours++;
                avgPos += boid.transform.position;
                avgDir += boid.velocity;
            }
            
        }
        if(neighbours > 0)
        {
            avgPos /= neighbours;
            velocity += (avgPos - transform.position) * Time.deltaTime * groupSteerForce;

            avgDir /= neighbours;
            velocity += (avgDir - velocity) * Time.deltaTime * alignSteerForce;
        }

        velocity += closeDir * Time.deltaTime * spreadSteerForce;

        if(velocity.magnitude > speed)
        {
           velocity = velocity.normalized* speed;
        }

        transform.position += velocity * Time.deltaTime;

        transform.forward = velocity;
    }

    bool DistanceLessThan(Vector3 a,Vector3 b, float d)
    {
        Vector3 vector = a - b;

        return vector.sqrMagnitude < Mathf.Pow(d,2);
    }
}
