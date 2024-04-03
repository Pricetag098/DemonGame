using DemonInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ChaosDemon : DemonFramework
{
    [SerializeField] float meleeStopingDistance;
    [SerializeField] ChaosStats normalStats, enragedStats;
    //ObjectPooler pooler;
    [SerializeField, Range(0, 1)] float enragePoint;

    float castTimer;
    [SerializeField] SoundPlayer enrageSound;

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

    [Header("Enrage Stats")]
    public int DemonEnrageAmount;

    ChaosStats activeStats;
    bool isEnraged;
    [Serializable]
    public class ChaosStats
    {
        public float meleeDamage;
        public float spellDamage;
        public float spellRadius;
        public AnimationCurve castInterval;
        public float meleeAggroRange;
        public float moveSpeed;
        public float stoppingDistance;
        public float strikeWarmUp;
    }
    void SetValues(ChaosStats stats)
    {
        activeStats = stats;
        _aiAgent.followSpeed = stats.moveSpeed;
        
    }

    #region OVERRIDE_FUNCTIONS
    public override void OnAwakened()
    {
        base.OnAwakened();

        m_obstacle = GetComponent<DestroyObstacle>();
        //pooler = GetComponent<ObjectPooler>();
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
        Dissolve();

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

        SetAnimationVariables();

        _aiAgent.LookDirection();

        PathFinding();

        //Chase the player at closeRange
        if(Vector3.SqrMagnitude(transform.position - CurrentTarget.position) < activeStats.meleeAggroRange * activeStats.meleeAggroRange)
        {
            _aiAgent.stopingDistance = meleeStopingDistance;
        }
        else
        {
            _aiAgent.stopingDistance = activeStats.stoppingDistance;
        }

        EnrageState();

        if(castTimer < 0)
        {
            //Cast

            //Vector3 randVal = UnityEngine.Random.insideUnitSphere * activeStats.spellRadius;
            //randVal.y = MathF.Abs(randVal.y);
            //Vector3 point = CurrentTarget.position + CurrentTarget.GetComponent<Rigidbody>().velocity * activeStats.strikeWarmUp + randVal;

            //pooler.Spawn().GetComponent<ChaosDemonSpell>().Spawn(activeStats.strikeWarmUp,point,activeStats.spellDamage);

            //castTimer = activeStats.castInterval.Evaluate(_health.health / _health.maxHealth);

        }
        castTimer -= Time.deltaTime;
    }

    private void EnrageState()
    {
        if(isEnraged == true) { return; }

        if (!isEnraged && _health.health / _health.maxHealth < enragePoint)
        {
            SetEnrageState();
            return;
        }

        if(_spawner.GetDemonQueueCount + _spawner.GetActiveDemonCount <= DemonEnrageAmount)
        {
            SetEnrageState();
            return;
        }
    }

    private void SetEnrageState()
    {
        SetValues(enragedStats);
        enrageSound.Play();
        isEnraged = true;
        Debug.Log("Demon is angy");
    }
    
    public override void OnSpawn(DemonType type, Transform target, SpawnType spawnType, bool inMap)
    {
        _aiAgent.SetFollowSpeed(0);
        _aiAgent.SetIsSpawned(false);
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
        isEnraged = false;
        DemonInMap = inMap;

        m_obstacle.SpawnReset();

        switch (spawnType)
        {
            case SpawnType.Default:
                _deathPoints.points = pointsOnDeath;
                _health.pointsOnHit = true;
                break;
            case SpawnType.Ritual:
                _deathPoints.points = 0;
                _health.pointsOnHit = false;
                break;
        }

        DemonMaterials.SetChaosMaterial(_skinnedMeshRenderer);

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
        SetValues(normalStats);
        castTimer = activeStats.castInterval.Evaluate(1);
        _aiAgent.canRotate = true;
    }
    public override void OnDeath()
    {
        _aiAgent.SetFollowSpeed(0);

        _animator.SetLayerWeight(_animator.GetLayerIndex("Upper"), 0);

        PlaySoundDeath();

        _isDead = true;

        Transform t = transform;
        t.position += new Vector3(0, 1, 0);
        _spawnerManager.SpawnBlessing(t);

        _aiAgent.canRotate = false;

        _ragdoll.ToggleRagdoll(true);

        _spawnerManager.DemonKilled();
    }
    public override void OnForcedDeath(bool ignoreImmunity)
    {
        
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

        _spawner.AddDemonBackToPool(_type, _spawnerManager);

        MarkForRemoval();
    }

    public override bool CheckToDespawn()
    {
        if(_isSpawned == false) { return false; }

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
        if (!_isSpawned)
            return;

        base.OnAttack();

        // deal damage
        Transform target = CurrentTarget;

        if (Vector3.Distance(target.position, transform.position) < _attackRange)
        {
            target.GetComponent<Health>().TakeDmg(activeStats.meleeDamage, HitType.Null);
            if (target.TryGetComponent<DamageIndicator>(out DamageIndicator damageIndicator))
            {
                damageIndicator.Indicate(transform);
            }
        }

        //unstuck resetting
        _aiAgent.ResetStuckTimer();
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

        if (CheckToDespawn() == true) // add timer to this
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
        _aiAgent.SetIsSpawned(true);
        SetValues(normalStats);
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
        if (IsAlive() == false) { return; }

        base.SetAnimationVariables();

        float evalSpeed = GetRange(_aiAgent.VelocityMag, 0, activeStats.moveSpeed);

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

    private void Dissolve()
    {
        if (_isDead == true)
        {
            if (_isRagdolled == false)
            {
                _isRagdolled = true;

                Transform t = transform;
                t.position = t.position + new Vector3(0, -1, 0);
                transform.position = t.position;
            }

            float fade = _deathFadeTimer.Time * fadeTimeMultiplier;

            Material[] skinMats = _skinnedMeshRenderer.materials;

            foreach (Material m in skinMats)
            {
                m.SetFloat("_AlphaClip", fade);
            }

            _skinnedMeshRenderer.materials = skinMats;

            if (_deathFadeTimer.TimeGreaterThan)
            {
                _isDead = false;

                _deathFadeTimer.ResetTimer(deathFadeTime);

                if (_spawnType == SpawnType.Default) { _spawnerManager.DemonKilled(); }

                _ragdoll.ToggleRagdoll(false);

                skinMats = _skinnedMeshRenderer.materials;

                foreach (Material m in skinMats)
                {
                    m.SetFloat("AlphaClip", 0);
                }

                _skinnedMeshRenderer.materials = skinMats;

                MarkForRemoval();
            }
        }
    }

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
