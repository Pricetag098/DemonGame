using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControllerDetection : MonoBehaviour
{

    [Header("ControllerSpriteReferences")]
    [SerializeField] GameObject square;
    [SerializeField] GameObject triangle;
    [SerializeField] GameObject leftBumper;

    [Header("KeyboardSpriteReferences")]
    [SerializeField] GameObject f;
    [SerializeField] GameObject e;
    [SerializeField] GameObject q;




    public void KeyboardSwap(bool status)
    {
        f.SetActive(status);
        q.SetActive(status);
        e.SetActive(status);
    }

    public void ControllerSwap(bool status)
    {
        square.SetActive(status);
        triangle.SetActive(status);
        leftBumper.SetActive(status);
    }

}
