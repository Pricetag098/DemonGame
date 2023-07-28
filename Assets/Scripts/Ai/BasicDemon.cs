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
        CalculateAndSetPath(_target);
        _agent.stoppingDistance = _stoppingDistance;


    }
    public override void Tick()
    {
        PathFinding();
    }
    public override void OnAttack()
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
    public override void OnHit()
    {
        
    }

    public override void PathFinding()
    {
        CalculateAndSetPath(_target);
    }
}
