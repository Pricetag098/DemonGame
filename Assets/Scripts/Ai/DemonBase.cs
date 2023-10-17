using DemonInfo;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.AI;

public class DemonBase : MonoBehaviour, IDemon
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

    [Header("Points")]
    [SerializeField] protected int pointsOnDeath;
    protected GrantPointsOnDeath _deathPoints;

    protected SpawnerManager _spawnerManager;
    [HideInInspector] public bool DemonInMap;

    [Header("Demon Type")]
    protected DemonType _type;
    protected SpawnType _spawnType;

    [Header("BaseStats")]
    [SerializeField] protected float _baseDamage;
    [SerializeField] protected float _baseHealth;
    [SerializeField] protected float _baseMoveSpeed;

    [Header("Stats")]
    [SerializeField] protected float _damage;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _attackSpeed;
    [SerializeField] protected float _attackRange;
    [SerializeField] protected float _stoppingDistance;

    [Header("Demon Sounds")]
    [SerializeField] SoundPlayer _soundPlayerIdle;
    [SerializeField] SoundPlayer _soundPlayerAttack;
    [SerializeField] SoundPlayer _soundPlayerAttackAmbience;
    [SerializeField] SoundPlayer _soundPlayerHit;
    [SerializeField] SoundPlayer _soundPlayerDeath;
    [SerializeField] SoundPlayer _soundPlayerFootsteps;

    [Header("Ai Pathing")]
    protected bool _calculatePath = false;
    protected Vector3 lastPos;
    //protected NavMeshAgent _agent;
    protected AiAgent _aiAgent;
    protected NavMeshPath _currentPath;

    protected int _currentUpdatedRound = 1;
    protected Vector3 spawpos = Vector3.zero;

    private void Awake()
    {
        //_agent = GetComponent<NavMeshAgent>();
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

        //_aiAgent.enabled = false;
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
        _aiAgent.followSpeed = 0;
        //_aiAgent.enabled = false;
        
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
        _aiAgent.followSpeed = 0;
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

        DemonSpawner.ActiveDemons.Add(this);

        PlaySoundIdle();

        PlayAnimation("Spawn");
    }
    public virtual void OnBuff() { }
    public virtual void OnDespawn(bool forcedDespawn = false)
    {
        _aiAgent.followSpeed = 0;
        //_aiAgent.enabled = false;

        SetAllColliders(false);

        if(forcedDespawn == true) _spawner.AddDemonBackToPool(_type, _spawnerManager);
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
    public virtual void DetectPlayer(bool active) { }
    public virtual void UpdateHealthToCurrentRound(int currentRound) { }

    protected Collider[] GetAllColliders()
    {
        return HelperFuntions.AllChildren<Collider>(transform).ToArray();
    }

    protected void SetAllColliders(bool active)
    {
        foreach(Collider c in _colliders)
        {
            c.enabled = active;
        }
    }

    public void ForcedDeath()
    {
        _aiAgent.followSpeed = 0;
        //_aiAgent.enabled = false;

        SetAllColliders(false);

        PlayAnimation("Death");

        PlaySoundDeath();
    }

    public virtual void OnFinishedSpawnAnimation() 
    {
        _aiAgent.followSpeed = _moveSpeed;
        _rb.isKinematic = false;
        _animator.applyRootMotion = false;
    }
    protected void OnFinishedDeathAnimation()
    {
        if (_spawnType == SpawnType.Default) { _spawnerManager.DemonKilled(); }

        _pooledObject.Despawn();
    }

    public void ApplyForce(Vector3 force, ForceMode mode = ForceMode.Impulse)
    {
        _rb.AddForce(force, mode);
    }

    public void PlayAnimation(string trigger)
    {
        _animator.SetTrigger(trigger);
    }

    public void PlaySoundIdle()
    {
        _soundPlayerIdle.Play();
    }

    public void PlaySoundAttack()
    {
        _soundPlayerAttack.Play();
        
    }

    public void PlaySoundAttackAmbience()
    {
        _soundPlayerAttackAmbience.Play();
    }

    public void PlaySoundHit()
    {
        _soundPlayerHit.Play();
    }

    public void PlaySoundDeath()
    {
        _soundPlayerDeath.Play();
    }

    protected void PlaySoundFootStep()
    {
        _soundPlayerFootsteps.Play();
    }
    public void AttackAnimation()
    {
        if (_animator.GetFloat("Speed") <= 0f)
        {
            if(!_animator.GetCurrentAnimatorStateInfo(1).IsName("Attack"))
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

    public void setSpawnPosition(Vector3 pos)
    {
        spawpos = pos;
    }

    #region Properties
    protected float DistanceToTargetNavmesh // gets path distance remaining to target
    {
        get
        {
            return _aiAgent.RemainingDistance;
        }
    }

    protected float DistanceToTargetUnits // gets world space distance remaining to target
    {
        get
        {
            return Vector3.Distance(_target.position, transform.position);
        }
    }
    #endregion

    #region Interface

    public void CalculateAndSetPath(Transform targetPos, bool active)
    {
        if (active == true)
        {
            //NavMeshPath path = new NavMeshPath();

            _aiAgent.UpdatePath(targetPos);

            //if(_aiAgent.CalculatePath(targetPos.position, path))
            //{
            //    _aiAgent.SetPath(path);
            //}

            //if (path.status == NavMeshPathStatus.PathComplete)
            //{
            //    _aiAgent.SetPath(path);
            //}

            _target = targetPos;
        }
    }

    public void StopPathing()
    {
        //_agent.isStopped = true;
        //_agent.ResetPath();
    }
    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }
    public Transform GetTarget()
    {
        return _target;
    }
    public void UpdateAttackSpeed(float amount)
    {
        _attackSpeed = amount;
    }
    public void SetAttackSpeed(float amount)
    {
        _attackSpeed += amount;
    }
    public void UpdateHealth(float amount)
    {
        _health.health += amount;
    }
    public void SetHealth(float amount)
    {
        _health.health = amount;
    }
    public void UpdateMaxHealth(float amount)
    {
        _health.maxHealth += amount;
    }
    public void SetMaxHealth(float amount)
    {
        _health.maxHealth = amount;
    }
    public void UpdateAttackRange(float amount)
    {
        _attackRange += amount;
    }
    public void SetAttackRange(float amount)
    {
        _attackRange = amount;
    }
    public void UpdateMoveSpeed(float amount)
    {
        _moveSpeed += amount;
    }
    public void SetMoveSpeed(float amount)
    {
        _moveSpeed = amount;
    }
    public void UpdateDamage(float amount)
    {
        _damage += amount;
    }
    public void SetDamage(float amount)
    {
        _damage = amount;
    }
    #endregion
}