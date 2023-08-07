using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class BasicDemon : DemonBase
{
    [Header("Demon Health Algorithm")]
    [SerializeField] int m_xAmountOfRounds;
    [SerializeField] float m_HealthToAdd;
    [SerializeField] float m_HealthMultiplier;

    public override void Setup()
    {
        _health.OnDeath += OnDeath;
        _health.OnHit += OnHit;
        _health.health = _baseHealth;
        _health.maxHealth = _baseHealth;

        _agent.stoppingDistance = _stoppingDistance;
    }
    public override void Tick()
    {
        PathFinding();
        LookAt();
    }
    public override void OnAttack()
    {
        // deal damage
    }
    public override void OnSpawn(Transform target)
    {
        CalculateStats(_spawner.currentRound);
        //CalculateAndSetPath(target);
        SetHealth(_health.maxHealth);
        transform.rotation = Quaternion.identity;
    }

    public override void OnRespawn()
    {
        _spawner.DemonRespawn(_type);
    }

    public override void OnDeath() // add back to pool of demon type
    {
        _pooledObject.Despawn();
        _spawner.DemonKilled();
        _agent.speed = 0;
    }
    public override void OnBuff()
    {
        // apply stat updates
    }
    public override void OnHit()
    {
        // do hit stuff
    }

    public override void PathFinding()
    {
        //CalculateAndSetPath(_target);
    }

    public override void CalculateStats(int round)
    {
        if(round != _currentUpdatedRound)
        {
            if (round <= m_xAmountOfRounds)
            {
                _health.maxHealth += m_HealthToAdd;
            }
            else { _health.maxHealth = _health.maxHealth * m_HealthMultiplier; }

            _currentUpdatedRound = round;
        }
    }
}
