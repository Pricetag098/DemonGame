using DemonInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserDemon : DemonFramework
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

    public bool DemonInMap;

    #region OVERRIDE_FUNCTIONS
    public override void OnAwakened()
    {
        base.OnAwakened();

        m_obstacle = GetComponent<DestroyObstacle>();
    }
    public override void OnStart()
    {
        base.OnStart();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        DetectTarget();

        if(DemonInMap == false) { m_obstacle.Detection(); }

        SetAnimationVariables();

        _aiAgent.LookDirection();

        PathFinding();
    }
    public override void OnSpawn(DemonType type, Transform target, SpawnType spawnType)
    {
        base.OnSpawn(type, target, spawnType);

        UpdateHealthToCurrentRound(_spawnerManager.currentRound);

        SetHealth(_health.maxHealth);

        _health.dead = false;

        SetMoveSpeed(type.SpeedType);

        _aiAgent.canRotate = true;
    }
    public override void OnDeath()
    {
        base.OnDeath();

        if (_spawnType == SpawnType.Ritual)
        {
            _spawnerManager.CurrentRitualOnDemonDeath();
        }

        if (SoulBox != null)
        {
            SoulBox.AddSoul();
        }

        _aiAgent.canRotate = false;
    }
    public override void OnForcedDeath()
    {
        base.OnForcedDeath();

        _spawner.AddDemonBackToPool(_type, _spawnerManager);
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
    }
    public override void OnAttack()
    {
        base.OnAttack();
    }
    public override void OnHit()
    {
        base.OnHit();
    }
    public override void PathFinding()
    {
        base.PathFinding();
    }
    public override void CalculateStats(int round)
    {
        base.CalculateStats(round);
    }
    public override void DetectTarget()
    {
        base.DetectTarget();
    }
    public override void CalculateAndSetPath()
    {
        base.CalculateAndSetPath();
    }
    public override void UpdateHealthToCurrentRound(int currentRound)
    {
        base.UpdateHealthToCurrentRound(currentRound);
    }
    public override void OnFinishedSpawnAnimation()
    {
        base.OnFinishedSpawnAnimation();
    }
    public override void OnFinishedDeathAnimation()
    {
        base.OnFinishedDeathAnimation();
    }
    public override void SetAnimationVariables()
    {
        base.SetAnimationVariables();

        float evalSpeed = GetRange(_aiAgent.VelocityMag, 0, speedProfile.maxSpeed);

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
    #endregion

    #region MOVESPEED_FUNCTIONS
    private void SetMoveSpeed(SpeedType type)
    {
        switch (type)
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
    #endregion

    #region HELPER_FUNCTIONS
    /// <summary>
    /// Returns Between 0 - 1 If Value is between Min and Max
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="destMin"></param>
    /// <param name="destMax"></param>
    /// <returns></returns>
    private float GetRange(float value, float min, float max, float destMin = 0, float destMax = 1) // can return less than 0 and greater than 1 if value is outside bounds of min and max
    {
        return destMin + (value - min) / (max - min) * (destMax - destMin);
    }
    #endregion

    #region GAMEOBJECT_FUNCTIONS
    private void OnDestroy()
    {
        _health.OnDeath -= OnDeath;
        _health.OnHit -= OnHit;
    }
    #endregion
}