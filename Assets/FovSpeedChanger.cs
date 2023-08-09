using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FovSpeedChanger : MonoBehaviour
{
    [SerializeField] AnimationCurve fovCurve;
    [SerializeField] Rigidbody rb;
    [SerializeField] Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cam.fieldOfView = fovCurve.Evaluate(rb.velocity.magnitude);
    }
}
