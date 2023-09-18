using DemonInfo;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemonBase : MonoBehaviour, IDemon
{
    [Header("Target")]
    [SerializeField] protected Transform _target;

    [Header("Spawner")]
    [SerializeField] protected DemonSpawner _spawner;

    protected SpawnerManager _spawnerManager;
    protected bool ritualSpawn;

    [Header("Demon Type")]
    [SerializeField] protected DemonType _type;

    [Header("BaseStats")]
    [SerializeField] protected float _baseDamage;
    [SerializeField] protected float _baseHealth;
    [SerializeField] protected float _baseMoveSpeed;
    [SerializeField] protected DemonSpeedProfile _speedProfile;

    [Header("Stats")]
    [SerializeField] protected float _damage;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _attackSpeed;
    [SerializeField] protected float _attackRange;
    [SerializeField] protected float _stoppingDistance;

    [Header("Demon Sounds")]
    [SerializeField] SoundPlayer _soundPlayerIdle;
    [SerializeField] SoundPlayer _soundPlayerAttack;
    [SerializeField] SoundPlayer _soundPlayerHit;
    [SerializeField] SoundPlayer _soundPlayerDeath;
    [SerializeField] SoundPlayer _soundPlayerFootsteps;

    [Header("AnimationCurves")]
    [SerializeField] protected AnimationCurve _moveSpeedCurve;

    [Header("Animator")]
    protected Animator _animator;

    [Header("Collider")]
    protected Collider[] _colliders;

    [Header("Rigidbody")]
    protected Rigidbody _rb;

    [Header("Ai Pathing")]
    protected bool _calculatePath = false;
    protected Vector3 lastPos;
    protected NavMeshAgent _agent;
    protected NavMeshPath _currentPath;

    protected Health _health;
    protected PooledObject _pooledObject;

    protected int _currentUpdatedRound = 1;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
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

        _agent.stoppingDistance = _stoppingDistance;
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
    public virtual void PathFinding(bool canPath) { }
    public virtual void OnDeath()
    {
        _agent.speed = 0;
        _agent.enabled = false;

        SetAllColliders(false);

        _spawner.ActiveDemons.Remove(this);

        _animator.SetTrigger("Death");

        PlaySoundDeath();
    }
    public virtual void OnSpawn(DemonType demon, Transform target, bool defaultSpawn = true)
    {
        _agent.speed = 0;
        _agent.enabled = true;
        SetAllColliders(true);

        _spawner.ActiveDemons.Add(this);

        PlaySoundIdle();
    }
    public virtual void OnBuff() { }
    public virtual void OnRespawn(bool defaultDespawn = true, bool forcedDespawn = false, bool ritualDespawn = false)
    {
        _agent.speed = 0;
        _agent.enabled = false;

        SetAllColliders(false);

        if (defaultDespawn == true)
        {
            _spawner.ActiveDemons.Remove(this);
            _spawner.AddDemonBackToPool(_type, _spawnerManager);
        }
        else if (forcedDespawn == true)
        {
            _spawner.AddDemonBackToPool(_type, _spawnerManager);
        }

        if (ritualDespawn == true) 
        {
            _spawnerManager.AddDemonBackToRitual(_type);
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

    protected void OnFinishedSpawnAnimation() 
    {
        _agent.speed = _moveSpeed;
    }
    protected void OnFinishedDeathAnimation()
    {
        _pooledObject.Despawn();

        if(ritualSpawn == false) { _spawnerManager.DemonKilled(); }
    }

    public void ApplyForce(Vector3 force, ForceMode mode = ForceMode.Impulse)
    {
        _rb.AddForce(force, mode);
    }

    public void PlayAnimation(string trigger)
    {
        _animator.SetTrigger(trigger);
    }

    protected void PlaySoundIdle()
    {
        _soundPlayerIdle.Play();
    }

    protected void PlaySoundAttack()
    {
        _soundPlayerAttack.Play();
    }

    protected void PlaySoundHit()
    {
        _soundPlayerHit.Play();
    }

    protected void PlaySoundDeath()
    {
        _soundPlayerDeath.Play();
    }

    protected void PlaySoundFootStep()
    {
        _soundPlayerFootsteps.Play();
    }

    #region Properties
    protected float DistanceToTargetNavmesh // gets path distance remaining to target
    {
        get
        {
            return _agent.remainingDistance;
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
    
    public NavMeshPath CalculatePath(Transform targetPos)
    {
        NavMeshPath path = new NavMeshPath();

        _agent.CalculatePath(targetPos.position, path);

        return path;
    }
    public void CalculateAndSetPath(Transform targetPos)
    {
        NavMeshPath path = new NavMeshPath();

        lastPos = targetPos.position;

        _agent.CalculatePath(lastPos, path);

        if(path.status == NavMeshPathStatus.PathComplete)
        {
            _agent.SetPath(path);
        }

        _target = targetPos;
    }
    
    public void StopPathing()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
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