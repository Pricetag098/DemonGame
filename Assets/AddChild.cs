using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddChild : MonoBehaviour
{
    [SerializeField] GameObject ball1;
    [SerializeField] GameObject ball2;

    public void AddChildren()
    {
        ball1.transform.parent = transform;
        if(ball2 != null)
        {
            ball2.transform.parent = transform;
        }
    }
}
