using DemonInfo;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BasicDemon : DemonBase
{
    [Header("Demon Pathing")]
    [SerializeField] float distanceToRespawn;

    [Header("Speed Profiles")]
    [HideInInspector] public SpeedType speedType;
    [SerializeField] DemonSpeedProfile walker;
    [SerializeField] DemonSpeedProfile jogger;
    [SerializeField] DemonSpeedProfile runner;

    [Header("Demon Health Algorithm")]
    [SerializeField] int m_xAmountOfRounds;
    [SerializeField] float m_HealthToAdd;
    [SerializeField] float m_HealthMultiplier;

    [Header("ObstacleDetection")]
    [SerializeField] DestroyObstacle m_obstacle;

    public override void OnAwakened()
    {
        m_obstacle = GetComponent<DestroyObstacle>();
    }
    public override void Setup()
    {
        UpdateHealthToCurrentRound(_spawnerManager.currentRound);
        base.Setup();
    }

    public override void Tick()
    {
        PathFinding(_agent.enabled);
        DetectPlayer(_agent.enabled);

        m_obstacle.Detection();

        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }
    public override void OnAttack()
    {
        base.OnAttack();

        // deal damage
        if(Vector3.Distance(_target.position,transform.position) < _attackRange)
            _target.GetComponent<Health>().TakeDmg(_damage);
    }
    public override void OnSpawn(DemonType demon,Transform target, bool defaultSpawn = true)
    {
        if(defaultSpawn == true) { ritualSpawn = false; }
        else { ritualSpawn = true; }

        base.OnSpawn(demon, target);
        UpdateHealthToCurrentRound(_spawnerManager.currentRound);
        CalculateAndSetPath(target);
        SetHealth(_health.maxHealth);
        SetMoveSpeed(demon.SpeedType);
        _health.dead = false;
    }
    public override void OnRespawn(bool defaultDespawn = true, bool forcedDespawn = false, bool ritualDespawn = false)
    {
        base.OnRespawn(defaultDespawn, forcedDespawn, ritualDespawn);

        
    }
    public override void OnDeath() // add back to pool of demon type
    {
        base.OnDeath();
        
        if(ritualSpawn == true)
        {
            _spawnerManager.CurrentRitualOnDemonDeath();
        }
    }
    public override void OnBuff()
    {
        // apply stat updates
    }
    public override void OnHit()
    {
        base.OnHit();
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
                PlayAnimation("Attack");
            }

            dist = DistanceToTargetNavmesh;

            if (dist > 100000) dist = 0;

            if(dist > distanceToRespawn)
            {
                OnRespawn();
            }
        }
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

    private void SetMoveSpeed(SpeedType speedType)
    {
        float maxspeed = 0;

        switch(speedType)
        {
            case SpeedType.Walker:
                _moveSpeed = walker.GetSpeed();
                maxspeed = walker.maxSpeed;
                break;
            case SpeedType.Jogger:
                _moveSpeed = jogger.GetSpeed();
                maxspeed = jogger.maxSpeed;
                break;
            case SpeedType.Runner:
                _moveSpeed = runner.GetSpeed();
                maxspeed = runner.maxSpeed;
                break;
        }

        float evalSpeed = GetRange(_moveSpeed, walker.minSpeed, runner.maxSpeed);

        _animator.SetFloat("Speed", evalSpeed);
    }

    private float GetRange(float value, float min, float max, float destMin = 0, float destMax = 1)
    {
        return destMin + ((value - min) / (max - min)) * (destMax - destMin);
    }

    

    private void OnDestroy()
    {
        _health.OnDeath -= OnDeath;
        _health.OnHit -= OnHit;
    }
}
