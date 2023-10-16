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
    [SerializeField] DemonSpeedProfile walker;
    [SerializeField] DemonSpeedProfile jogger;
    [SerializeField] DemonSpeedProfile runner;
    DemonSpeedProfile speedProfile;
    [HideInInspector] public SpeedType SpeedType;

    [Header("Demon Health Algorithm")]
    [SerializeField] int m_xAmountOfRounds;
    [SerializeField] float m_HealthToAdd;
    [SerializeField] float m_HealthMultiplier;

    [Header("SoulBox")]
    public SoulBox SoulBox;

    [Header("ObstacleDetection")]
    private DestroyObstacle m_obstacle;

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
        DetectPlayer(_aiAgent.enabled);

        //if(DemonInMap == false) m_obstacle.Detection();

        SetAnimationVariables();
    }
    public override void OnAttack()
    {
        base.OnAttack();

        // deal damage
        if (Vector3.Distance(_target.position, transform.position) < _attackRange)
        {
            _target.GetComponent<Health>().TakeDmg(_damage);
            if (_target.TryGetComponent<DamageIndicator>(out DamageIndicator damageIndicator))
            {
                damageIndicator.Indicate(this.transform);
            }
        }
    }
    public override void OnSpawn(DemonType demon,Transform target, SpawnType type)
    {
        base.OnSpawn(demon, target, type);

        UpdateHealthToCurrentRound(_spawnerManager.currentRound);

        SetHealth(_health.maxHealth);
        _health.dead = false;

        SetMoveSpeed(demon.SpeedType);

        DemonInMap = false;
    }
    public override void OnDespawn(bool forcedDespawn = false)
    {
        base.OnDespawn(forcedDespawn);
    }
    public override void OnDeath() // add back to pool of demon type
    {
        base.OnDeath();
        
        if(_spawnType == SpawnType.Ritual)
        {
            _spawnerManager.CurrentRitualOnDemonDeath();
        }

        if(SoulBox != null)
        {
            SoulBox.AddSoul();
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

        //_aiAgent.enabled = true;
        CalculateAndSetPath(_target, _aiAgent.enabled);
    }

    public override void PathFinding()
    {
        CalculateAndSetPath(_target, _aiAgent.enabled);
    }

    public override void DetectPlayer(bool active)
    {
        if(active == true)
        {
            float dist = DistanceToTargetUnits;

            if (dist < _attackRange)
            {
                AttackAnimation();
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

    private void SetAnimationVariables()
    {
        float evalSpeed = GetRange(_aiAgent.VelocityMag, 0, speedProfile.maxSpeed); // returns between 0 - 1
        _animator.SetFloat("Speed", evalSpeed);

        if (evalSpeed <= 0f)
        {
            if (!_animator.GetCurrentAnimatorStateInfo(1).IsName("Attack"))
            {
                _animator.SetLayerWeight(_animator.GetLayerIndex("Upper"), 0.0f);
            }
        }
        else
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("StandingAttack"))
            {
                _animator.SetLayerWeight(_animator.GetLayerIndex("Upper"), 1.0f);
            }
        }
    }

    /// <summary>
    /// Returns Between 0 - 1 If Value is between Min and Max
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="destMin"></param>
    /// <param name="destMax"></param>
    /// <returns></returns>
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
