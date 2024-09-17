using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierTracker : MonoBehaviour
{
    public int barrierLimit;

    private int barrierCount;

    public void ResetCount()
    {
        barrierCount = 0;
    }

    public void Rebuilt()
    {
        barrierCount++;
    }

    public bool CanRebuild()
    {
        if (barrierCount > barrierLimit)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
