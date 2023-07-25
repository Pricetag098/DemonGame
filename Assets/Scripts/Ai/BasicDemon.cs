using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class BasicDemon : DemonBase
{
    [Header("Demon")]
    [SerializeField] int wave;

    public override void Setup()
    {
        CalculateStats(wave);
    }
    public override void Tick()
    {
        PathFinding();
    }
    public override void Attack()
    {
        
    }
    public override void OnSpawn()
    {
        CalculateStats(wave);
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
