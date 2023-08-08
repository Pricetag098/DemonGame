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
        UpdateHealthToCurrentRound(_spawner.currentRound);
        base.Setup();
    }
    public override void Tick()
    {
        PathFinding(_agent.enabled);
        DetectPlayer();

        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }
    public override void OnAttack()
    {
        // deal damage
    }
    public override void OnSpawn(Transform target)
    {
        base.OnSpawn(target);
        UpdateHealthToCurrentRound(_spawner.currentRound);
        CalculateAndSetPath(target);
        SetHealth(_health.maxHealth);
    }
    public override void OnRespawn()
    {
        _spawner.DemonRespawn(_type);
    }
    public override void OnDeath() // add back to pool of demon type
    {
        _agent.speed = 0;
        _agent.enabled = false;
        _collider.enabled = false;
        
        _animator.SetTrigger("Death");
    }
    public override void OnBuff()
    {
        // apply stat updates
    }
    public override void OnHit()
    {
        // do hit stuff
    }

    public override void PathFinding(bool active)
    {
        if(active == true)
        {
            CalculateAndSetPath(_target);
        }
    }

    public override void DetectPlayer()
    {
        // if within range trigger animation
    }

    public override void CalculateStats(int round)
    {
        if (round <= m_xAmountOfRounds)
        {
            _health.maxHealth += m_HealthToAdd;
        }
        else { _health.maxHealth = _health.maxHealth * m_HealthMultiplier; }
    }

    public override void UpdateHealthToCurrentRound(int currentRound)
    {
        if(currentRound != _currentUpdatedRound)
        {
            int num = currentRound - _currentUpdatedRound;

            for (int round = 0 + _currentUpdatedRound; round < num + _currentUpdatedRound; round++)
            {
                CalculateStats(round);
            }

            _currentUpdatedRound = currentRound;
        }
    }

    private void OnDestroy()
    {
        _health.OnDeath -= OnDeath;
        _health.OnHit -= OnHit;
    }
}
