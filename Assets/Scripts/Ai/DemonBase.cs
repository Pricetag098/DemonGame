using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBase : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] protected GameObject _target;

    [Header("Stats")]
    [SerializeField] protected float _damage;
    [SerializeField] protected float _attackSpeed;
    [SerializeField] protected float _health;
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _attackRange;
    [SerializeField] protected float _stoppingDistance;
    [SerializeField] protected bool calculatePath = false;


}
