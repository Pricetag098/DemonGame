using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicDemon : DemonBase, IDemon
{
    private NavMeshAgent agent;
    private NavMeshPath currentPath;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.speed = _moveSpeed;
        agent.stoppingDistance = _stoppingDistance;
        calculatePath = true;
    }
    private void Update()
    {
        PathFinding(calculatePath);
    }
    public void PathFinding(bool calculate)
    {
        if (calculatePath == true)
        {
            Transform pathingTarget = _target.transform;

            currentPath = CalculatePath(pathingTarget);

            agent.SetPath(currentPath);

            calculatePath = false;
        }
    }

    NavMeshPath CalculatePath(Transform targetPos)
    {
        NavMeshPath path = new NavMeshPath();

        agent.CalculatePath(targetPos.position, path);

        return path;
    }

    float DistanceToTarget(Transform targetPos)
    {
        return agent.remainingDistance;
    }
    

    #region HelperFunctions
    public void UpdateTarget(GameObject newTarget)
    { 
        _target = newTarget;
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
