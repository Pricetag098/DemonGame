using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailUpdating : MonoBehaviour
{
    public List<TrailRenderer> leftHandTrails;

    public List<TrailRenderer> rightHandTrails;

    public void TurnOnLeft()
    {
        foreach (TrailRenderer l in leftHandTrails)
        {
            l.enabled = true;
        }
    }

    public void TurnOffLeft()
    {
        foreach (TrailRenderer l in leftHandTrails)
        {
            l.enabled = false;
        }
    }

    public void TurnOffRight()
    {
        foreach (TrailRenderer l in rightHandTrails)
        {
            l.enabled = false;
        }
    }

    public void TurnOnRight()
    {
        foreach (TrailRenderer l in rightHandTrails)
        {
            l.enabled = true;
        }
    }
}
