using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FovSpeedChanger : MonoBehaviour,IDataPersistance<PlayerSettings>
{
    [SerializeField] AnimationCurve fovCurve;
    [SerializeField] Rigidbody rb;
    [SerializeField] Camera cam;
    public float fov;
    public void LoadData(PlayerSettings data)
    {
        fov = data.fov;
    }

    public void SaveData(ref PlayerSettings data)
    {
        data.fov = fov;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vel = rb.velocity;
        vel.y = 0;
        cam.fieldOfView = fov + fovCurve.Evaluate(vel.magnitude);
    }
}
