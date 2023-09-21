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
    [SerializeField] protected SpawnType _spawnType;

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

    [SerializeField] protected Vector3 spawpos = Vector3.zero;

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

        _agent.enabled = false;
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
    public virtual void PathFinding() { }
    public virtual void OnDeath()
    {
        _agent.speed = 0;
        _agent.enabled = false;

        SetAllColliders(false);

        _spawner.ActiveDemons.Remove(this);

        PlayAnimation("Death");

        PlaySoundDeath();
    }
    public virtual void OnSpawn(DemonType demon, Transform target, SpawnType type)
    {
        _agent.speed = 0;
        _target = target;
        _spawnType = type;
        _rb.isKinematic = true;

        SetAllColliders(true);

        _spawner.ActiveDemons.Add(this);

        PlaySoundIdle();

        PlayAnimation("Spawn");
    }
    public virtual void OnBuff() { }
    public virtual void OnDespawn(bool forcedDespawn = false)
    {
        _agent.speed = 0;
        _agent.enabled = false;

        SetAllColliders(false);

        if(forcedDespawn == true) _spawner.AddDemonBackToPool(_type, _spawnerManager);
        else
        {
            switch (_spawnType)
            {
                case SpawnType.Default:
                    _spawner.ActiveDemons.Remove(this);
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
        _agent.speed = 0;
        _agent.enabled = false;

        SetAllColliders(false);

        PlayAnimation("Death");

        PlaySoundDeath();
    }

    public virtual void OnFinishedSpawnAnimation() 
    {
        _agent.speed = _moveSpeed;
        _rb.isKinematic = false;
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

    public void SetNavmeshPosition(Vector3 pos)
    {
        transform.position = pos;
        _agent.nextPosition = pos;
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

    public void setSpawnPosition(Vector3 pos)
    {
        spawpos = pos;
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
    public void CalculateAndSetPath(Transform targetPos, bool active)
    {
        if(active == true)
        {
            NavMeshPath path = new NavMeshPath();

            lastPos = targetPos.position;

            _agent.CalculatePath(lastPos, path);

            if (path.status == NavMeshPathStatus.PathComplete)
            {
                _agent.SetPath(path);
            }

            _target = targetPos;
        }
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