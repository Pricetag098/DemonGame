using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IDemon
{
    void SetTarget(GameObject newTarget);
    GameObject GetTarget();
    void UpdateAttackSpeed(float amount);
    void SetAttackSpeed(float amount);
    void UpdateHealth(float amount);
    void SetHealth(float amount);
    void UpdateMaxHealth(float amount);
    void SetMaxHealth(float amount);
    void UpdateAttackRange(float amount);
    void SetAttackRange(float amount);
    void UpdateMoveSpeed(float amount);
    void SetMoveSpeed(float amount);
    void UpdateDamage(float amount);
    void SetDamage(float amount);
    void PathFinding(bool calculatePath);
    NavMeshPath CalculatePath(Transform targetPos);
    void StopPathing();
}