using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindRigidBody : MonoBehaviour
{
    [Min(0.01f)]public float freq = 0;
    public float amp = 5;
    public float radius = 100;

    float windStrength;
    RaycastHit hit;
    Collider[] hitColliders;
    Rigidbody rb;
    void Update()
    {

        windStrength = Mathf.Sin(Time.time/freq) * amp;

        hitColliders = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(transform.forward * windStrength);
            }
        }
    }
}
