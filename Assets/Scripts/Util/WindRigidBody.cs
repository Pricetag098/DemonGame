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
    Rigidbody[] bodys;
    Rigidbody rb;
    [SerializeField] LayerMask layer;

    private void Start()
    {
        hitColliders = Physics.OverlapSphere(transform.position, radius);
        List<Rigidbody> list = new List<Rigidbody>();
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].TryGetComponent(out Rigidbody rb))
            {
                list.Add(rb);
            }
        }
        bodys = list.ToArray();
    }

    void FixedUpdate()
    {

        windStrength = Mathf.Sin(Time.time/freq) * amp;
        foreach(Rigidbody rb in bodys)
        {
            rb.AddForce(transform.forward * windStrength);
        }
        
    }
}
