using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicDemon : MonoBehaviour, IDemon
{
    private NavMeshAgent agent;
    [SerializeField] private GameObject target;

    [Header("Stats")]
    public float _damage;
    public float _attackSpeed;
    public float _health;
    public float _maxHealth;
    public float _moveSpeed;
    public float _attackRange;
    public float _stoppingDistance;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.speed = _moveSpeed;
        agent.stoppingDistance = _stoppingDistance;

    }
    private void Update()
    {
        PathFinding();
    }
    public void PathFinding()
    {

    }

    #region HelperFunctions
    public void UpdateTarget(GameObject newTarget)
    { 
        target = newTarget;
    }
    public void UpdateAttackSpeed(float amount)
    { 
        _attackSpeed += amount;
    }
    public void UpdateHealth(float amount)
    { 
        if (_health > _maxHealth)
        { 
            _health = _maxHealth;
        }
        else _health = amount; 
    }
    public void UpdateMaxHealth(float amount)
    {
        _maxHealth = amount; 
    }

    public void UpdateAttackRange(float amount)
    {
        _attackRange = amount;
    }

    public void UpdateMoveSpeed(float amount)
    {
        _moveSpeed = amount;
    }

    public void UpdateDamage(float amount)
    {
        _damage = amount;
    }
    #endregion
}
