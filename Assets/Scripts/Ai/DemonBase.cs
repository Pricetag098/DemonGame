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
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected float _attackSpeed;
    [SerializeField] protected float _attackRange;
    [SerializeField] protected float _stoppingDistance;

    [Header("AnimationCurves")]
    [SerializeField] protected AnimationCurve _damageCurve;
    [SerializeField] protected AnimationCurve _maxHealthCurve;
    [SerializeField] protected AnimationCurve _moveSpeedCurve;

    [Header("Ai Pathing")]
    [SerializeField] protected bool _calculatePath = false;
    protected NavMeshAgent _agent;
    protected NavMeshPath _currentPath;

    protected Health _health;
    protected PooledObject _pooledObject;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();
        _spawner = FindObjectOfType<DemonSpawner>();
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

    public virtual void Setup() { }
    public virtual void Tick() { }
    public virtual void OnAttack() { }
    public virtual void OnHit() { }
    public virtual void PathFinding() { }
    public virtual void OnDeath() { }
    public virtual void OnSpawn(Transform target) { }
    public virtual void OnBuff() { }

    #region Properties
    protected float DistanceToTarget // gets path distance remaining to target
    {
        get
        {
            return _agent.remainingDistance;
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
    public void CalculateStats(int round)
    {
        _damage = _damageCurve.Evaluate(round) + _baseDamage;
        _maxHealth = _maxHealthCurve.Evaluate(round) + _baseHealth;
        //_moveSpeed = _moveSpeedCurve.Evaluate(round) + _baseMoveSpeed;

        _agent.speed = _moveSpeed;
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
        if (_health.health > _maxHealth) _health.health = _maxHealth;
    }
    public void UpdateMaxHealth(float amount)
    {
        _maxHealth += amount;
    }
    public void SetMaxHealth(float amount)
    {
        _maxHealth = amount;
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