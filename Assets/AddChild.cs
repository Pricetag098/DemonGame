using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddChild : MonoBehaviour
{
    [SerializeField] GameObject ball1;
    [SerializeField] GameObject ball2;

    public void AddChildren()
    {
        Vector3 ball1pos = transform.position;
        ball1.transform.parent = transform;
        ball1.transform.position = ball1pos;
        if(ball2 != null)
        {
            Vector3 ball2pos = transform.position;
            ball2.transform.parent = transform;
            ball2.transform.position = ball2pos;
        }
    }
}
