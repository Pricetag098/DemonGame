using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DestroyObstacle : MonoBehaviour
{
    [SerializeField] private float CheckDistance = 1f;
    [SerializeField] private float AttackDelay = 1f;
    [SerializeField] private int DestructibleAttackDamage = 1;
    [SerializeField] private LayerMask DestructibleLayers;

    //private NavMeshAgent Agent;
    private AiAgent Agent;
    private LesserDemon demon;
    private NavMeshPath OrigianlPath;
    private Timer timer;

    private void Awake()
    {
        Agent = GetComponent<AiAgent>();
        demon = GetComponent<LesserDemon>();
        timer = new Timer(AttackDelay, true);
    }

    private bool checkForObjects;
    private bool foundObject;
    private DestrcutibleObject obj;

    //resets all variables when demon returns from pool
    public void SpawnReset()
    {
        foundObject = false;
        obj = null;
    }

    public void Detection()
    {
        if(foundObject == false)
        {
            checkForObjects = CheckForDistructibleObjects();
        }

        if (checkForObjects == false && foundObject == false) return;

        foundObject = true;

        if (timer.TimeGreaterThan)
        {
            if(obj is not null)
            {
                demon.CurrentAttackStateAnimation();
            }
        }
    }

    public void BarrierTakeDmg()
    {
        if (obj is not null)
        {
            obj.TakeDamage(DestructibleAttackDamage);

            if (obj.Health <= 0)
            {
                Agent.canMove = true;
                obj = null;
                demon.SetDemonInMap(true);
            }
        }
    }
    
    private bool CheckForDistructibleObjects()
    {
        RaycastHit hit;
        Agent.RaycastMoveDirection(CheckDistance, out hit, DestructibleLayers);

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<DestrcutibleObject>(out DestrcutibleObject d))
            {
                if (d.Health > 0)
                {
                    Agent.canMove = false;
                    obj = d;
                    return true;
                }
                else
                {
                    demon.SetDemonInMap(true);
                }
            }
        }

        return false;
    }
}
