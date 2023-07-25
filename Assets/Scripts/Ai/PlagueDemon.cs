using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueDemon : DemonBase
{
    public override void Setup()
    {
        
    }
    public override void Tick()
    {
        
    }
    public override void Attack()
    {
        
    }
    public override void OnSpawn()
    {

    }
    public override void OnDeath()
    {

    }
    public override void PathFinding()
    {
        if (_calculatePath == true)
        {
            Transform pathingTarget = _target.transform;

            _currentPath = CalculatePath(pathingTarget);

            _agent.SetPath(_currentPath);

            _calculatePath = false;
        }
    }
}
