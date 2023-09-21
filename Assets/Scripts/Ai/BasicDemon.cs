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
    [SerializeField] DemonSpeedProfile speedProfile;
    [HideInInspector] public SpeedType SpeedType;
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
        //PathFinding(_agent.enabled);
        DetectPlayer(_agent.enabled);

        m_obstacle.Detection();

        SetAnimationMoveSpeed();
    }
    public override void OnAttack()
    {
        base.OnAttack();

        // deal damage
        if(Vector3.Distance(_target.position,transform.position) < _attackRange)
            _target.GetComponent<Health>().TakeDmg(_damage);
    }
    public override void OnSpawn(DemonType demon,Transform target, SpawnType type)
    {
        base.OnSpawn(demon, target, type);

        UpdateHealthToCurrentRound(_spawnerManager.currentRound);

        SetHealth(_health.maxHealth);
        _health.dead = false;

        SetMoveSpeed(demon.SpeedType);
    }
    public override void OnDespawn(bool forcedDespawn = false)
    {
        base.OnDespawn();
    }
    public override void OnDeath() // add back to pool of demon type
    {
        base.OnDeath();
        
        if(_spawnType == SpawnType.Ritual)
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
    public override void OnFinishedSpawnAnimation()
    {
        base.OnFinishedSpawnAnimation();

        SetNavmeshPosition(spawpos);

        //_agent.enabled = true;
        CalculateAndSetPath(_target, _agent.enabled);

        Debug.Log("Finished Spawn Override");
    }

    public override void PathFinding()
    {
        CalculateAndSetPath(_target, _agent.enabled);
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

            if(dist > distanceToRespawn) OnDespawn();
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

    private void SetMoveSpeed(SpeedType type)
    {
        switch(type)
        {
            case SpeedType.Walker:
                speedProfile = walker;
                _moveSpeed = speedProfile.GetSpeed();
                break;
            case SpeedType.Jogger:
                speedProfile = jogger;
                _moveSpeed = speedProfile.GetSpeed();
                break;
            case SpeedType.Runner:
                speedProfile = runner;
                _moveSpeed = speedProfile.GetSpeed();
                break;
        }

        SpeedType = type;
    }

    private void SetAnimationMoveSpeed()
    {
        float evalSpeed = GetRange(_agent.velocity.magnitude, 0, speedProfile.maxSpeed);

        _animator.SetFloat("Speed", evalSpeed);
    }

    private float GetRange(float value, float min, float max, float destMin = 0, float destMax = 1)
    {
        return destMin + (value - min) / (max - min) * (destMax - destMin);
    }

    

    private void OnDestroy()
    {
        _health.OnDeath -= OnDeath;
        _health.OnHit -= OnHit;
    }
}
