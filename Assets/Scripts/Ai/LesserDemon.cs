using DemonInfo;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class LesserDemon : DemonFramework
{
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

    #region OVERRIDE_FUNCTIONS
    public override void OnAwakened()
    {
        base.OnAwakened();

        m_obstacle = GetComponent<DestroyObstacle>();
    }
    public override void OnStart()
    {
        _pooledObject = GetComponent<PooledObject>();

        _health.health = _health.maxHealth;

        _health.OnDeath += OnDeath;
        _health.OnHit += OnHit;

        _aiAgent.stopingDistance = _stoppingDistance;
    }
    public override void OnUpdate()
    {
        DeathFade();

        if (IdleSoundTimer.TimeGreaterThan)
        {
            IdleSoundInterval(IdleSoundTimer);

        }
        if (WhileMovingSoundTimer.TimeGreaterThan)
        {
            WhileMovingSoundInterval(WhileMovingSoundTimer);
        }

        if (SampleNavmeshPosition() == false && _isDead == false)
        {
            OnDespawn();
            return;
        }

        if (DetectTarget() == false) { return; }

        if(GetDemonInMap == false) { m_obstacle.Detection(); }

        SetAnimationVariables();

        _aiAgent.LookDirection();

        PathFinding();

        UpdateAgentNearby(GetAgent.Grid.cells.UpdateObjectAndGetSurroudingObjects(GetAgent));
    }
    public override void OnSpawn(DemonType type, Transform target, SpawnType spawnType, bool inMap)
    {
        _aiAgent.SetFollowSpeed(0);
        CurrentTarget = target;
        _spawnType = spawnType;
        _type = type;
        _rb.isKinematic = true;
        _animator.enabled = true;
        _animator.applyRootMotion = true;
        _isRemoved = false;
        _isDead = false;
        _isRagdolled = false;
        _isSpawned = false;
        DemonInMap = inMap;

        _attachments.ResetAllAttachments();
        _attachments.RandomAttachments();

        switch (spawnType)
        {
            case SpawnType.Default:
                _deathPoints.points = pointsOnDeath;

                DemonMaterials.SetDefaultSpawningMaterial(_skinnedMeshRenderer);

                foreach (var obj in _attachments.ReturnActiveObjects())
                {
                    DemonMaterials.SetDefaultAttachmentMaterial(obj);
                }

                break;
            case SpawnType.Ritual:
                _deathPoints.points = 0;

                DemonMaterials.SetRitualMaterial(_skinnedMeshRenderer);

                foreach (var obj in _attachments.ReturnActiveObjects())
                {
                    DemonMaterials.SetRitualAttachmentMaterial(obj);
                }

                break;
        }

        _aiAgent.Initalised();

        GetAgent.Grid.cells.Insert(GetAgent);

        _animationOverrides.SelectController(_animator);

        SetAllColliders(true);

        DemonSpawner.ActiveDemons.Add(this);

        PlayAnimation("Spawn");

        UpdateHealthToCurrentRound(SpawnerManager.currentRound);

        SetHealth(_health.maxHealth);

        _health.dead = false;

        SetMoveSpeed(type.SpeedType);

        _aiAgent.canRotate = true;
    }
    public override void OnDeath()
    {
        _aiAgent.SetFollowSpeed(0);

        _animator.SetLayerWeight(_animator.GetLayerIndex("Upper"), 0);

        PlaySoundDeath();

        _isDead = true;

        switch(_spawnType)
        {
            case SpawnType.Default:
                Transform t = transform;
                t.position += new Vector3(0, 1, 0);
                _spawnerManager.GetBlessingChance(t, GetDemonInMap);

                _spawnerManager.DemonKilled();

                break;
            case SpawnType.Ritual:
                _spawnerManager.CurrentRitualOnDemonDeath();
                break;
        }

        if (SoulBox != null)
        {
            SoulBox.AddSoul();
        }

        _aiAgent.canRotate = false;

        _ragdoll.ToggleRagdoll(true);
    }
    public override void OnForcedDeath(bool ignoreImmunity)
    {
        _aiAgent.SetFollowSpeed(0);

        SetAllColliders(false);

        _animator.SetLayerWeight(_animator.GetLayerIndex("Upper"), 0);

        _isDead = true;

        _ragdoll.ToggleRagdoll(true);

        //MarkForRemoval();
    }
    public override void OnForcedDespawn()
    {
        _aiAgent.SetFollowSpeed(0);

        SetAllColliders(false);

        _spawner.AddDemonBackToPool(_type, _spawnerManager);

        MarkForRemoval();
    }
    public override void OnDespawn()
    {
        _aiAgent.SetFollowSpeed(0);

        SetAllColliders(false);

        switch (_spawnType)
        {
            case SpawnType.Default:
                _spawner.AddDemonBackToPool(_type, _spawnerManager);
                break;
            case SpawnType.Ritual:
                _spawnerManager.AddDemonBackToRitual(_type);
                break;
        }

        MarkForRemoval();
    }

    public override bool CheckToDespawn()
    {
        if(_spawnType == SpawnType.Ritual) { return false; }

        float dist = DistanceToTargetNavmesh;

        if (dist > 100000) dist = 0;

        if (dist > _distanceToRespawn)
        {
            return true;
        }

        return false;
    }
    public override void OnAttack()
    {
        base.OnAttack();

        // deal damage
        Transform target = CurrentTarget;

        if (Vector3.Distance(target.position, transform.position) < _attackRange)
        {
            target.GetComponent<Health>().TakeDmg(_damage, HitType.Null);
            if (target.TryGetComponent<DamageIndicator>(out DamageIndicator damageIndicator))
            {
                damageIndicator.Indicate(transform);
            }
        }
    }
    public override void OnHit()
    {
        base.OnHit();
    }
    public override void PathFinding()
    {
        _aiAgent.UpdatePath(CurrentTarget);
    }
    public override void CalculateStats(int round)
    {
        if (round <= m_xAmountOfRounds)
        {
            _health.maxHealth += m_HealthToAdd;
        }
        else { _health.maxHealth = _health.maxHealth * m_HealthMultiplier; }
    }
    public override bool DetectTarget()
    {
        base.DetectTarget();

        float dist = DistanceToTargetUnits;

        if (dist < _attackRange)
        {
            CurrentAttackStateAnimation();
        }

        if(CheckToDespawn() == true) // add timer to this
        {
            OnDespawn();
        }

        return true;
    }
    public override void CalculateAndSetPath()
    {
        base.CalculateAndSetPath();
    }
    public override void UpdateHealthToCurrentRound(int currentRound)
    {
        if (currentRound != _currentUpdatedRound)
        {
            for (int round = _currentUpdatedRound; round < currentRound; round++)
            {
                CalculateStats(round);
            }

            _currentUpdatedRound = currentRound;
        }
    }
    public override void OnFinishedSpawnAnimation()
    {
        _aiAgent.SetFollowSpeed(_moveSpeed);
        _rb.isKinematic = false;
        _animator.applyRootMotion = false;
        _isSpawned = true;
    }
    public override void OnFinishedDeathAnimation()
    {
        if (_spawnType == SpawnType.Default) { _spawnerManager.DemonKilled(); }

        MarkForRemoval();
    }
    public override void SetAnimationVariables()
    {
        if(IsAlive() == false) { return; }

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