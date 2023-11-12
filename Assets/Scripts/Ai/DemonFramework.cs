using BlakesSpatialHash;
using DemonInfo;
using System.Collections.Generic;
using UnityEngine;

public class DemonFramework : MonoBehaviour
{
    #region TARGET
    /// <summary>
    /// Current Target
    /// </summary>
    public Transform CurrentTarget { get; private set; }
    #endregion

    #region VISUALS
    /// <summary>
    /// Attachemnt Component for Random Visuals
    /// </summary>
    [Header("Attachments")]
    protected DemonAttachments _attachments;

    /// <summary>
    /// Skinned Mesh Renderer
    /// </summary>
    [Header("SkinnedMeshedRenderer")]
    [SerializeField] protected SkinnedMeshRenderer _skinnedMeshRenderer;
    #endregion

    #region SPAWNING
    /// <summary>
    /// Accessor To The Spawning System
    /// </summary>
    [Header("Spawner")]
    protected DemonSpawner _spawner;

    /// <summary>
    /// Accessor To The Demon Manager
    /// </summary>
    protected SpawnerManager _spawnerManager;

    /// <summary>
    /// Object Pooler
    /// </summary>
    [Header("Pooled Object")]
    protected PooledObject _pooledObject;

    /// <summary>
    /// Demon Type
    /// </summary>
    [Header("Demon Type")]
    protected DemonType _type;

    /// <summary>
    /// Spawn Type
    /// </summary>
    protected SpawnType _spawnType;
    #endregion

    #region DEATH
    protected bool _isDead;
    #endregion

    #region ANIMATION
    /// <summary>
    /// 
    /// </summary>
    [Header("Animator")]
    protected Animator _animator;

    /// <summary>
    /// Overrides For Animation Controller
    /// </summary>
    [Header("Animation Overwrite")]
    protected DemonAnimationOverrides _animationOverrides;
    #endregion

    #region COLLIDERS & PHYSICS
    /// <summary>
    /// Array of Colliders
    /// </summary>
    [Header("Collider")]
    protected Collider[] _colliders;

    /// <summary>
    /// Rigidbody
    /// </summary>
    [Header("Rigidbody")]
    protected Rigidbody _rb;
    #endregion

    #region HEALTH
    /// <summary>
    /// Health
    /// </summary>
    [Header("Health")]
    protected Health _health;
    #endregion

    #region POINT_GAIN
    /// <summary>
    /// Points Gained On Death
    /// </summary>
    [Header("Points")]
    [SerializeField] protected int pointsOnDeath;
    protected GrantPointsOnDeath _deathPoints;
    #endregion

    #region STATS
    [Header("Stats")]
    [SerializeField] protected float _damage;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _attackRange;
    [SerializeField] protected float _stoppingDistance;
    [SerializeField] protected float _distanceToRespawn;
    #endregion

    #region SOUNDS
    [Header("Demon Sounds")]
    [SerializeField] SoundPlayer _soundPlayerIdle;
    [SerializeField] SoundPlayer _soundPlayerAttack;
    [SerializeField] SoundPlayer _soundPlayerAttackAmbience;
    [SerializeField] SoundPlayer _soundPlayerHit;
    [SerializeField] SoundPlayer _soundPlayerDeath;
    [SerializeField] SoundPlayer _soundPlayerFootsteps;

    private Timer IdleSoundTimer;
    [Header("Idle Sound")]
    [SerializeField] float minTimeInterval;
    [SerializeField] float maxTimeInterval;
    #endregion

    #region AI
    [Header("Ai Agent")]
    protected AiAgent _aiAgent;
    #endregion

    #region INITALISE
    private void Awake()
    {
        _aiAgent = GetComponent<AiAgent>();
        _health = GetComponent<Health>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _deathPoints = GetComponent<GrantPointsOnDeath>();
        _attachments = GetComponent<DemonAttachments>();
        _animationOverrides = GetComponent<DemonAnimationOverrides>();
        _spawner = FindObjectOfType<DemonSpawner>();
        _spawnerManager = FindObjectOfType<SpawnerManager>();
        _colliders = GetAllColliders();

        IdleSoundTimer = new Timer(Random.Range(minTimeInterval, maxTimeInterval));

        OnAwakened();
    }

    private void Start()
    {
        OnStart();
    }
    #endregion

    #region UPDATE
    private void Update()
    {
        
    }
    #endregion

    #region VIRTUAL_FUNCTIONS
    public virtual void OnAwakened() { }
    public virtual void OnStart() 
    { 
        _pooledObject = GetComponent<PooledObject>();

        _health.health = _health.maxHealth;

        _health.OnDeath += OnDeath;
        _health.OnHit += OnHit;

        _aiAgent.stopingDistance = _stoppingDistance;
    }
    public virtual void OnUpdate() 
    {
        if (IdleSoundTimer.TimeGreaterThan)
        {
            IdleSoundInterval(IdleSoundTimer);
        }
    }
    public virtual void OnSpawn(DemonType type, Transform target, SpawnType spawnType)
    {
        _aiAgent.SetFollowSpeed(0);
        CurrentTarget = target;
        _spawnType = spawnType;
        _type = type;
        _rb.isKinematic = true;
        _animator.applyRootMotion = true;
        _isDead = false;

        switch (spawnType)
        {
            case SpawnType.Default:
                _deathPoints.points = pointsOnDeath;
                break;
            case SpawnType.Ritual:
                _deathPoints.points = 0;
                break;
        }

        _animationOverrides.SelectController(_animator);

        SetAllColliders(true);

        foreach (var obj in _attachments.ReturnActiveObjects())
        {
            DemonMaterials.SetAttachmentMaterial(obj);
        }

        DemonMaterials.SetDefaultSpawningMaterial(_skinnedMeshRenderer);

        PlayAnimation("Spawn");
    }
    public virtual void OnDeath() 
    {
        _aiAgent.SetFollowSpeed(0);

        RemoveFromSpatialHash();

        SetAllColliders(false);

        //DemonSpawner.ActiveDemons.Remove(this);

        _animator.SetLayerWeight(_animator.GetLayerIndex("Upper"), 0);

        PlayAnimation("Death");
        PlaySoundDeath();
    }
    public virtual void OnForcedDeath() { }
    public virtual void OnDespawn() 
    {
        _aiAgent.SetFollowSpeed(0);

        SetAllColliders(false);

        RemoveFromSpatialHash();

        switch (_spawnType)
        {
            case SpawnType.Default:
                _spawner.AddDemonBackToPool(_type, _spawnerManager);
                break;
            case SpawnType.Ritual:
                _spawnerManager.AddDemonBackToRitual(_type);
                break;
        }

        _pooledObject.Despawn();
    }
    public virtual void OnForcedDespawn() 
    {
        _aiAgent.SetFollowSpeed(0);

        SetAllColliders(false);

        RemoveFromSpatialHash();

        _spawner.AddDemonBackToPool(_type, _spawnerManager);

        _pooledObject.Despawn();
    }
    public virtual void OnAttack() { }
    public virtual void OnHit() { }
    public virtual void PathFinding() 
    {
        _aiAgent.UpdatePath(CurrentTarget, out bool valid);
    }
    public virtual void CalculateStats(int round) { }
    public virtual void DetectTarget() { }
    public virtual void CalculateAndSetPath() { }
    public virtual void UpdateHealthToCurrentRound(int currentRound) { }
    public virtual void OnFinishedSpawnAnimation()
    {
        _aiAgent.SetFollowSpeed(_moveSpeed);
        _rb.isKinematic = false;
        _animator.applyRootMotion = false;
    }
    public virtual void OnFinishedDeathAnimation() 
    {
        if (_spawnType == SpawnType.Default) { _spawnerManager.DemonKilled(); }

        _isDead = true;

        //_pooledObject.Despawn();
    }
    public virtual void SetAnimationVariables() { }
    public virtual bool CheckToDespawn() 
    {
        float dist = DistanceToTargetNavmesh;

        if (dist > 100000) dist = 0;

        if (dist > _distanceToRespawn)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region AI_FUNCTIONS
    /// <summary>
    /// Returns The Agent and Spatialhash Component
    /// </summary>
    public AiAgent GetAgent { get { return _aiAgent; } }

    /// <summary>
    /// Returns the Remaining Distance of the Path
    /// </summary>
    protected float DistanceToTargetNavmesh { get { return _aiAgent.RemainingDistance; } }

    /// <summary>
    /// Returns world space distance to target
    /// </summary>
    protected float DistanceToTargetUnits { get { return Vector3.Distance(CurrentTarget.position, transform.position); } }

    /// <summary>
    /// Sets Nearby Spatial Hash OBJECTS
    /// </summary>
    /// <param name="objs"></param>
    public void UpdateAgentNearby(List<SpatialHashObject> objs) { _aiAgent.SetNearbyAgents(objs); }

    /// <summary>
    /// Removes OBJECT from Spatial Hash pool
    /// </summary>
    public void RemoveFromSpatialHash() { _aiAgent.RemoveFromSpatialHash(); }
    #endregion

    #region HEALTH_FUNCTIONS
    public Health GetHealth { get { return _health; } }
    public void SetHealth(float amount)
    {
        _health.health = amount;
    }
    public bool IsAlive()
    {
        if(_health.dead == false) { return true; }

        return false;
    }
    #endregion

    #region ANIMATOR_FUNCTIONS
    public Animator GetAnimator { get { return _animator; } }
    public void PlayAnimation(string trigger) { _animator.SetTrigger(trigger); }
    public void CurrenAttackStateAnimation()
    {
        if (_animator.GetFloat("Speed") <= 0f)
        {
            if (!_animator.GetCurrentAnimatorStateInfo(1).IsName("Attack"))
            {
                PlayAnimation("StandingAttack");
                return;
            }
        }

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("StandingAttack"))
        {
            PlayAnimation("Attack");

        }
    }
    public void SetAttackOverride()
    {
        _animator.runtimeAnimatorController = _animationOverrides.SetOverrideController();
    }

    #endregion

    #region RIGIDBODY_FUNCTIONS
    public Rigidbody GetRigidbody { get { return _rb; } }

    public void ApplyForce(Vector3 force, ForceMode mode = ForceMode.Impulse)
    {
        _rb.AddForce(force, mode);
    }
    #endregion

    #region SOUND_FUNCTIONS
    public void PlaySoundIdle() { _soundPlayerIdle.Play(); }

    public void PlaySoundAttack() { _soundPlayerAttack.Play(); }

    public void PlaySoundAttackAmbience() { _soundPlayerAttackAmbience.Play(); }

    public void PlaySoundHit() { _soundPlayerHit.Play(); }

    public void PlaySoundDeath() { _soundPlayerDeath.Play(); }

    protected void PlaySoundFootStep() { _soundPlayerFootsteps.Play(); }

    public void IdleSoundInterval(Timer timer)
    {
        timer.ResetTimer(Random.Range(minTimeInterval, maxTimeInterval));
        PlaySoundIdle();
    }
    #endregion

    #region COLLIDER_FUNCTIONS
    protected Collider[] GetAllColliders()
    {
        return HelperFuntions.AllChildren<Collider>(transform).ToArray();
    }

    protected void SetAllColliders(bool active)
    {
        foreach (Collider c in _colliders)
        {
            c.enabled = active;
        }
    }
    #endregion

    #region SPAWNING_FUNCTIONS
    
    #endregion

    #region DEATH_FUNCTIONS
    public bool CanDespawn()
    {
        if(_isDead == true)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region OBJECT_POOLER
    public void DespawnObject()
    {
        _pooledObject.Despawn();
    }
    #endregion
}