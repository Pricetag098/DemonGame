using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class BasicDemon : DemonBase
{
    [Header("Demon")]
    private float attackTimer;
    [SerializeField] private float attackInterval;

    [Header("Demon Pathing")]
    [SerializeField] float distanceToRespawn;

    [Header("Demon Health Algorithm")]
    [SerializeField] int m_xAmountOfRounds;
    [SerializeField] float m_HealthToAdd;
    [SerializeField] float m_HealthMultiplier;

    [Header("ObstacleDetection")]
    [SerializeField] DestroyObstacle m_obstacle;

    private delegate void DestroyBarrier();
    private DestroyBarrier m_barrier;

    private bool respawning = false;

    public override void OnAwakened()
    {
        m_obstacle = GetComponent<DestroyObstacle>();
    }
    public override void Setup()
    {
        UpdateHealthToCurrentRound(_spawner.currentRound);
        base.Setup();
    }
    public override void Tick()
    {
        attackTimer += Time.deltaTime;

        PathFinding(_agent.enabled);
        DetectPlayer(_agent.enabled);

        //m_obstacle.Detection();

        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }
    public override void DoDamage()
    {
        _playerHealth.health -= _damage;
    }
    public override void OnAttack()
    {
        if(HelperFuntions.TimerGreaterThan(attackTimer, attackInterval))
        {
            PlayAnimation("Attack");
        }
    }
    public override void OnSpawn(Transform target)
    {
        base.OnSpawn(target);
        UpdateHealthToCurrentRound(_spawner.currentRound);
        CalculateAndSetPath(target);
        SetHealth(_health.maxHealth);

        respawning = false;
    }
    public override void OnRespawn()
    {
        _agent.speed = 0;
        _agent.enabled = false;
        _collider.enabled = false;

        _spawner.DemonRespawn(_type, respawning);

        _pooledObject.Despawn();
        
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

    public override void DetectPlayer(bool active)
    {
        if(active == true)
        {
            float dist = DistanceToTargetUnits;

            if (dist < _attackRange)
            {
                OnAttack();
            }

            //if(dist > distanceToRespawn)
            //{
            //    OnRespawn();
            //}
        }
    }
    public override void CalculateStats(int round)
    {
        if (round <= m_xAmountOfRounds)
        {
            _health.maxHealth += m_HealthToAdd;
        }
        else { _health.maxHealth = _health.maxHealth * m_HealthMultiplier; }

        //_moveSpeed = _moveSpeedCurve.Evaluate(round) + _baseMoveSpeed;
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
