using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IDemon
{
    void SetHealth(float amount);
    public void CalculateStats(int round);
    void CalculateAndSetPath(Transform targetPos);
}