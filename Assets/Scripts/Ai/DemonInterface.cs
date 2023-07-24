using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IDemon
{
    void UpdateTarget(GameObject newTarget);
    void UpdateAttackSpeed(float amount);
    void UpdateHealth(float amount);
    void UpdateMaxHealth(float amount);
    void UpdateAttackRange(float amount);
    void UpdateMoveSpeed(float amount);
    void UpdateDamage(float amount);
    void PathFinding(bool calculatePath);
}