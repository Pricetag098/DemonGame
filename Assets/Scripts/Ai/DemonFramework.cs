using BlakesSpatialHash;
using DemonInfo;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    /// Skinned Mesh Renderer
    /// </summary>
    [Header("SkinnedMeshedRenderer")]
    [SerializeField] protected SkinnedMeshRenderer _skinnedMeshRenderer;

    protected DemonAttachments _attachments;
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

    protected bool _isSpawned;
    #endregion

    #region DEATH
    protected LesserDemonRagdoll _ragdoll;
    private bool _isRagdolled;

    protected bool _isRemoved;
    protected bool _isDead;

    [SerializeField] private float deathFadeTime;
    [SerializeField] private float fadeTimeMultiplier;
    private Timer _deathFadeTimer;
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
    [SerializeField] SoundPlayer _soundPlayerWhileMoving;
    [SerializeField] SoundPlayer _soundPlayerAttack;
    [SerializeField] SoundPlayer _soundPlayerAttackAmbience;
    [SerializeField] SoundPlayer _soundPlayerHit;
    [SerializeField] SoundPlayer _soundPlayerDeath;
    [SerializeField] SoundPlayer _soundPlayerFootsteps;

    private Timer IdleSoundTimer;
    [Header("Idle Sound")]
    [SerializeField] float minTimeInterval;
    [SerializeField] float maxTimeInterval;

    private Timer WhileMovingSoundTimer;
    [Header("While-Moving Sound")]
    [SerializeField] float whileMovingMinTimeInterval;
    [SerializeField] float whileMovingMaxTimeInterval;
    #endregion

    #region AI
    [Header("Ai Agent")]
    protected AiAgent _aiAgent;
    #endregion

    #region WORLD
    /// <summary>
    /// Returns If Demon is in the Map
    /// </summary>
    [SerializeField] private bool DemonInMap;

    private bool _isOnNavmesh;
    #endregion

    #region INITALISE
    private void Awake()
    {
        _aiAgent = GetComponent<AiAgent>();
        _health = GetComponent<Health>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _deathPoints = GetComponent<GrantPointsOnDeath>();
        _animationOverrides = GetComponent<DemonAnimationOverrides>();
        _spawner = FindObjectOfType<DemonSpawner>();
        _spawnerManager = FindObjectOfType<SpawnerManager>();
        _ragdoll = GetComponent<LesserDemonRagdoll>();
        _attachments = GetComponent<DemonAttachments>();
        _colliders = GetAllColliders();

        IdleSoundTimer = new Timer(Random.Range(minTimeInterval, maxTimeInterval));
        WhileMovingSoundTimer = new Timer(Random.Range(whileMovingMinTimeInterval, whileMovingMaxTimeInterval));
        _deathFadeTimer = new Timer(deathFadeTime);

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
        if (WhileMovingSoundTimer.TimeGreaterThan)
        {
            WhileMovingSoundInterval(WhileMovingSoundTimer);
        }

        if (SampleNavmeshPosition() == false && _isDead == false)
        {
            MarkForRemoval();
        }

        DeathFade();
    }

    public void DeathFade()
    {
        if (_isDead == true)
        {
            if (_isRagdolled == false)
            {
                _isRagdolled = true;
                _ragdoll.ToggleRagdoll(true);

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

            foreach(GameObject g in _attachments.ReturnActiveObjects())
            {
                if(g.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                {
                    renderer.material.SetFloat("_AlphaClip", fade);
                }
            }

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

                foreach (GameObject g in _attachments.ReturnActiveObjects())
                {
                    if (g.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                    {
                        renderer.material.SetFloat("_AlphaClip", 0);
                    }
                }

                MarkForRemoval();
            }
        }
    }
    public virtual void OnSpawn(DemonType type, Transform target, SpawnType spawnType, bool inMap)
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

        DemonMaterials.SetDefaultSpawningMaterial(_skinnedMeshRenderer);

        DemonSpawner.ActiveDemons.Add(this);

        PlayAnimation("Spawn");
    }
    public virtual void OnDeath() 
    {
        _aiAgent.SetFollowSpeed(0);

        RemoveFromSpatialHash();

        _animator.SetLayerWeight(_animator.GetLayerIndex("Upper"), 0);

        PlaySoundDeath();

        _isDead = true;
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

        MarkForRemoval();
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
        _aiAgent.UpdatePath(CurrentTarget);
    }
    public virtual void CalculateStats(int round) { }
    public virtual bool DetectTarget() { return true; }
    public virtual void CalculateAndSetPath() { }
    public virtual void UpdateHealthToCurrentRound(int currentRound) { }
    public virtual void OnFinishedSpawnAnimation()
    {
        _aiAgent.SetFollowSpeed(_moveSpeed);
        _rb.isKinematic = false;
        _animator.applyRootMotion = false;
        _isSpawned = true;
    }
    public virtual void OnFinishedDeathAnimation() 
    {
        if (_spawnType == SpawnType.Default) { _spawnerManager.DemonKilled(); }

        MarkForRemoval();
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
    public void CurrentAttackStateAnimation()
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

    public void PlaySoundWhileMoving() { _soundPlayerWhileMoving.Play(); }

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

    public void WhileMovingSoundInterval(Timer timer)
    {
        timer.ResetTimer(Random.Range(whileMovingMinTimeInterval, whileMovingMaxTimeInterval));
        PlaySoundWhileMoving();
    }
    #endregion

    #region COLLIDER_FUNCTIONS
    protected Collider[] GetAllColliders()
    {
        return HelperFuntions.AllChildren<Collider>(transform, true).ToArray();
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
    public void MarkForRemoval()
    {
        if(_isRemoved == false)
        {
            _isRemoved = true;
            DemonSpawner.ActiveDemonsToRemove.Add(this);
        }
    }
    #endregion

    #region OBJECT_POOLER
    public void DespawnObject()
    {
        _pooledObject.Despawn();
    }
    #endregion

    #region WORLD_FUNCTIONS
    public void SetDemonInMap(bool active)
    {
        DemonInMap = active;
    }
    public bool GetDemonInMap
    {
        get { return DemonInMap; }
    }

    private float onNavmeshDistance = 0.7f;

    public bool SampleNavmeshPosition()
    {
        if(NavMesh.SamplePosition(transform.position, out NavMeshHit hit, onNavmeshDistance, NavMesh.AllAreas))
        {
            _isOnNavmesh = true;
            return true;
        }

        _isOnNavmesh = false;
        return false;
    }
    #endregion
}