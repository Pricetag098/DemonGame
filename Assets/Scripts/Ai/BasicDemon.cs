using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class BasicDemon : DemonBase
{
    public override void Setup()
    {
        _agent.speed = _moveSpeed;
        _agent.stoppingDistance = _stoppingDistance;
        _calculatePath = true;
    }
    public override void Tick()
    {
        PathFinding(_calculatePath);
    }
}
