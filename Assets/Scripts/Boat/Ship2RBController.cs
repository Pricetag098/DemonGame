using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship2RBController : MonoBehaviour
{
    Rigidbody rb;
    public float acceleration = 1.0f;
    public float angularAcceleration = 1;

    void Start()
    {
        //  t = GetComponent<Transform> (); 
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        {
            float translation = -Input.GetAxis("Vertical") * acceleration;
            float rotation = Input.GetAxis("Horizontal") * angularAcceleration;
            translation *= Time.deltaTime;
            rotation *= Time.deltaTime;


            {
                rb.velocity = acceleration * transform.forward * Time.deltaTime;
                transform.Translate(0, 0, translation);
                transform.Rotate(0, rotation, 0);
            }

        }
    }

}
