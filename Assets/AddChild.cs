using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddChild : MonoBehaviour
{
    [SerializeField] GameObject ball1;
    [SerializeField] GameObject ball2;
    [SerializeField] GameObject fakeball1;
    [SerializeField] GameObject fakeball2;


    public void AddChildren()
    {
        ball1.gameObject.SetActive(false);
        fakeball1.gameObject.SetActive(true);
        if (ball2 != null)
        {
            ball2.gameObject.SetActive(false);
            fakeball2.gameObject.SetActive(true);
        }
    }
}
