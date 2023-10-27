using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBlade : MonoBehaviour
{
    [SerializeField]Vector3 rotation;

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += rotation * Time.deltaTime;
    }
}
