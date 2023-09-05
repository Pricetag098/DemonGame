using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : MonoBehaviour
{
    public GameObject completionObject;

    public void CorrectPainting()
    {
        completionObject.SetActive(true);
    }

    public void FailedPainting()
    {
        completionObject.SetActive(false);
    }
}
