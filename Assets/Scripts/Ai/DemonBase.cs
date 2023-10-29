using BlakesSpatialHash;
using DemonInfo;
using System.Collections.Generic;
using UnityEngine;

public class DemonBase : MonoBehaviour
{
    [Header("Target")]
    protected Transform _target;

    [Header("Spawner")]
    protected DemonSpawner _spawner;

    [Header("Attachments")]
    protected DemonAttachments _attachments;

    [Header("Animator")]
    protected Animator _animator;

    [Header("Animation Overwrite")]
    protected DemonAnimationOverrides _animationOverrides;

    [Header("Collider")]
    protected Collider[] _colliders;

    [Header("Rigidbody")]
    protected Rigidbody _rb;

    [Header("Health")]
    protected Health _health;

    [Header("Pooled Object")]
    protected PooledObject _pooledObject;

    [Header("SkinnedMeshedRenderer")]
    [SerializeField] protected SkinnedMeshRenderer _skinnedMeshRenderer;

    [Header("Points")]
    [SerializeField] protected int pointsOnDeath;
    protected GrantPointsOnDeath _deathPoints;

    protected SpawnerManager _spawnerManager;
    [HideInInspector] public bool DemonInMap;

    [Header("Demon Type")]
    protected DemonType _type;
    protected SpawnType _spawnType;

    [Header("Stats")]
    [SerializeField] protected float _damage;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _attackRange;
    [SerializeField] protected float _stoppingDistance;

    [Header("Demon Sounds")]
    [SerializeField] SoundPlayer _soundPlayerIdle;
    [SerializeField] SoundPlayer _soundPlayerAttack;
    [SerializeField] SoundPlayer _soundPlayerAttackAmbience;
    [SerializeField] SoundPlayer _soundPlayerHit;
    [SerializeField] SoundPlayer _soundPlayerDeath;
    [SerializeField] SoundPlayer _soundPlayerFootsteps;

    [Header("Ai Agent")]
    protected AiAgent _aiAgent;

    protected int _currentUpdatedRound = 1;
    protected Vector3 spawpos = Vector3.zero;

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

        OnAwakened();
    }

    private void Start()
    {
        Setup();
        _pooledObject = GetComponent<PooledObject>();
    }
    private void Update()
    {
        Tick();
    }
    
    public virtual void OnAwakened() { }
    public virtual void Setup()
    {
        _health.health = _health.maxHealth;

        _health.OnDeath += OnDeath;
        _health.OnHit += OnHit;

        _aiAgent.stopingDistance = _stoppingDistance;
    }
    public virtual void Tick() { }
    public virtual void OnAttack()
    {
        PlaySoundAttack();
    }
    public virtual void OnHit()
    {
        PlaySoundHit();
    } 
    public virtual void PathFinding() { }
    public virtual void OnDeath()
    {
        _aiAgent.SetFollowSpeed(0);
        RemoveFromSpatialHash();

        SetAllColliders(false);

        switch(_spawnType)
        {
            case SpawnType.Default:
                
                break;
            case SpawnType.Ritual:
                
                break;
        }

    
        Transform t = transform;
        t.position += new Vector3(0, 1, 0);
        _spawnerManager.GetBlessingChance(t, DemonInMap);

        DemonSpawner.ActiveDemons.Remove(this);

        _animator.SetLayerWeight(_animator.GetLayerIndex("Upper"), 0);

        PlayAnimation("Death");
        PlaySoundDeath();
    }
    public virtual void OnSpawn(DemonType demon, Transform target, SpawnType type)
    {
        _aiAgent.SetFollowSpeed(0);
        _target = target;
        _spawnType = type;
        _type = demon;
        _rb.isKinematic = true;
        _animator.applyRootMotion = true;

        switch (type)
        {
            case SpawnType.Default:
                _deathPoints.points = pointsOnDeath;
                break;
            case SpawnType.Ritual:
                _deathPoints.points = 0;
                DemonInMap = true;
                break;
        }

        _animationOverrides.SelectController(_animator);

        SetAllColliders(true);

        _attachments.ResetAllAttachments();
        _attachments.RandomAttachments();

        foreach(var obj in _attachments.ReturnActiveObjects())
        {
            DemonMaterials.SetAttachmentMaterial(obj);
        }

        DemonMaterials.SetDefaultSpawningMaterial(_skinnedMeshRenderer);

        DemonSpawner.ActiveDemons.Add(this);

        PlaySoundIdle();

        PlayAnimation("Spawn");
    }
    public virtual void OnBuff() { }
    public virtual void OnDespawn(bool forcedDespawn = false)
    {
        _aiAgent.SetFollowSpeed(0);

        SetAllColliders(false);

        RemoveFromSpatialHash();

        if (forcedDespawn == true) _spawner.AddDemonBackToPool(_type, _spawnerManager);
        else
        {
            switch (_spawnType)
            {
                case SpawnType.Default:
                    DemonSpawner.ActiveDemons.Remove(this);
                    _spawner.AddDemonBackToPool(_type, _spawnerManager);
                    break;
                case SpawnType.Ritual:
                    _spawnerManager.AddDemonBackToRitual(_type);
                    break;
            }
        }
        
        _pooledObject.Despawn();
    }
    public virtual void CalculateStats(int round) { }
    public virtual void DetectPlayer() { }
    public virtual void UpdateHealthToCurrentRound(int currentRound) { }

    public void ForcedDeath()
    {
        _aiAgent.SetFollowSpeed(0);

        SetAllColliders(false);

        RemoveFromSpatialHash();

        PlayAnimation("Death");

        PlaySoundDeath();
    }

    public void setSpawnPosition(Vector3 pos)
    {
        spawpos = pos;
    }

    #region AI FUNCTIONS
    /// <summary>
    /// Returns The Agent Component
    /// </summary>
    public AiAgent GetAgent { get { return _aiAgent; } }

    /// <summary>
    /// Returns the Remaining Distance of the Path
    /// </summary>
    protected float DistanceToTargetNavmesh { get { return _aiAgent.RemainingDistance; } }

    /// <summary>
    /// Returns world space distance to target
    /// </summary>
    protected float DistanceToTargetUnits { get { return Vector3.Distance(_target.position, transform.position); } }

    /// <summary>
    /// Calculates and Sets a New Path
    /// </summary>
    /// <param name="targetPos"></param>
    public void CalculateAndSetPath(Transform targetPos)
    {
        _aiAgent.UpdatePath(targetPos);
        
        _target = targetPos;
        
    }

    /// <summary>
    /// Returns SpatialHashObject
    /// </summary>
    /// <returns></returns>
    public SpatialHashObject GetSpatialHashObject() { return _aiAgent; }

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

    public bool HealthStatus()
    {
        return _health.dead;
    }

    #endregion

    #region ANIMATOR_FUNCTIONS
    public Animator GetAnimator { get { return _animator; } }
    public void PlayAnimation(string trigger) { _animator.SetTrigger(trigger); }

    public void AttackAnimation()
    {
        if (_animator.GetFloat("Speed") <= 0f)
        {
            if (!_animator.GetCurrentAnimatorStateInfo(1).IsName("Attack"))
            {
                PlayAnimation("StandingAttack");

            }
        }
        else
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("StandingAttack"))
            {
                PlayAnimation("Attack");

            }
        }
    }

    public void SetAttackOverride()
    {
        _animator.runtimeAnimatorController = _animationOverrides.SetOverrideController();
    }

    public virtual void OnFinishedSpawnAnimation()
    {
        _aiAgent.SetFollowSpeed(_moveSpeed);
        _rb.isKinematic = false;
        _animator.applyRootMotion = false;
    }
    protected void OnFinishedDeathAnimation()
    {
        if (_spawnType == SpawnType.Default) { _spawnerManager.DemonKilled(); }

        _pooledObject.Despawn();
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

    public void PlaySoundDeath() {_soundPlayerDeath.Play(); }

    protected void PlaySoundFootStep() {  _soundPlayerFootsteps.Play(); }
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

    public void SetDemonInMap(bool active)
    {
        DemonInMap = active;
    }

    #endregion
}