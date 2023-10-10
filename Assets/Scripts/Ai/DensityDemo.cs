using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DensityDemo : MonoBehaviour
{
    DensityDemo[] others;
    [SerializeField] Transform target;
    [SerializeField] float followSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float radius;
    [SerializeField] float pushForce;
    [SerializeField] float damping;
    [SerializeField] float sidedamping;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        others = FindObjectsOfType<DensityDemo>();   
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = rb.velocity;
    }

    private void FixedUpdate()
    {
        rb.AddForce(GetPushForce() * pushForce);
        

        Vector3 toTarget = (target.position - transform.position).normalized;
        if(Vector3.Dot(toTarget, rb.velocity) < followSpeed)
        {
            rb.AddForce(toTarget * acceleration);
        }
        else
        {
            rb.AddForce(-rb.velocity * damping);
        }
        Vector3 side = Vector3.Cross(toTarget, Vector3.up);
        rb.AddForce(side * Vector3.Dot(rb.velocity,side) *sidedamping);
    }

    float SmoothingVal(float radius,float distance)
    {
        float val = Mathf.Max(0,radius-distance);
        return val * val * val;
    }
    Vector3 GetPushForce()
    {
        Vector3 force = Vector3.zero;
        foreach(DensityDemo other in others)
        {
            if (other == this)
                continue;
            Vector3 dirTo = transform.position - other.transform.position;
            float dist = dirTo.magnitude;
            dirTo /= dist;

            force += dirTo * SmoothingVal(radius, dist);

        }

        return force;   


    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, GetPushForce());
    }

}
