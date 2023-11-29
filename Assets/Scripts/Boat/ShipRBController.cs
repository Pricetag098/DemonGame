using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRBController : MonoBehaviour
{
    Rigidbody rb;

    public float speed = 10.0f;
    public float rotationSpeed = 10.0f;

    public float acceleration = 0;
    public float angularAcceleration = 1;

    void Start()
    {
        //  t = GetComponent<Transform> (); 
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        {
            float translation = Input.GetAxis("Vertical") * speed;
            float rotation = Input.GetAxis("Horizontal") * (0.2f) * rotationSpeed;
            translation *= Time.deltaTime;
            rotation *= Time.deltaTime;


            {
                rb.velocity = acceleration * transform.forward * Time.deltaTime;
                transform.Translate(0, 0, translation);
                transform.Rotate(0, rotation, 0);
            }

          /* {
               /* // movement
                if (Input.GetKey(KeyCode.W)) //forward
                {
                    rb.velocity += acceleration * transform.forward * (0.5f) * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.S)) //backwards
                {
                    rb.velocity += acceleration * transform.forward * (0.5f) * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    rb.angularVelocity += (0.5f) * angularAcceleration * transform.up * Time.deltaTime;
                    //transform.Rotate(0, 0.75f, 0);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    rb.angularVelocity -= (0.5f) * angularAcceleration * transform.up * Time.deltaTime;
                    //transform.Rotate(0, -0.75f, 0);
                }
           
            } */
        }
    }
}