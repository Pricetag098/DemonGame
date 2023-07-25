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
        _agent.stoppingDistance = _stoppingDistance;

        CalculateStats(wave);
    }
    public override void Tick()
    {
        PathFinding();
    }
    public override void Attack()
    {
        // deal damage
    }
    public override void OnSpawn()
    {
        CalculateStats(wave);
        
        _health = _maxHealth;
        _calculatePath = true;
    }
    public override void OnDeath()
    {
        // add back to pool of demon type
    }
    public override void OnBuff()
    {
        // apply stat updates
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
