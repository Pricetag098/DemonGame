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

    [Header("Demon Type")]
    [SerializeField] protected DemonType _type;

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

    [Header("AnimationCurves")]
    [SerializeField] protected AnimationCurve _moveSpeedCurve;

    [Header("Animator")]
    protected Animator _animator;

    [Header("Collider")]
    protected Collider _collider;

    [Header("Rigidbody")]
    protected Rigidbody _rb;

    [Header("Ai Pathing")]
    protected bool _calculatePath = false;
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
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
        _spawner = FindObjectOfType<DemonSpawner>();

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
        LookAt(_agent.enabled);
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
    public virtual void OnAttack() { }
    public virtual void OnHit() { } 
    public virtual void PathFinding(bool canPath) { }
    public virtual void OnDeath() { }
    public virtual void OnSpawn(Transform target)
    {
        _agent.speed = 0;
        _agent.enabled = true;
        _collider.enabled = true;
        transform.rotation = Quaternion.identity;
    }
    public virtual void OnBuff() { }
    public virtual void OnRespawn() { }
    public virtual void CalculateStats(int round) { }
    public virtual void DetectPlayer(bool active) { }
    public virtual void UpdateHealthToCurrentRound(int currentRound) { }

    protected void LookAt(bool active)
    {
        if(active == true)
        {
            transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));
        }
    }
    protected void OnFinishedSpawnAnimation() 
    {
        _agent.speed = _moveSpeed;
    }
    protected void OnFinishedDeathAnimation()
    {
        _pooledObject.Despawn();
        //_spawner.DemonKilled();
    }

    public void PlayAnimation(string trigger)
    {
        _animator.SetTrigger(trigger);
    }


    #region Properties
    protected float DistanceToTargetNavmesh // gets path distance remaining to target
    {
        get
        {
            return _agent.remainingDistance;
        }
    }

    protected float DistanceToTargetUnits
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

        _agent.CalculatePath(targetPos.position, path);

        _agent.SetPath(path);

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