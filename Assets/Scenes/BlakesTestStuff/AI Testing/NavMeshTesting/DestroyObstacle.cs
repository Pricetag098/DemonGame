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
    private DemonBase demon;
    private NavMeshPath OrigianlPath;
    private Timer timer;

    private void Awake()
    {
        Agent = GetComponent<AiAgent>();
        demon = GetComponent<DemonBase>();
        timer = new Timer(AttackDelay, true);
    }

    private bool checkForObjects;
    private bool foundObject;
    private DestrcutibleObject obj;

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
                demon.AttackAnimation();
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
                Agent.enabled = true;
                Agent.canMove = true;
                obj = null;
                demon.DemonInMap = true;
            }
        }
    }

    private bool CheckForDistructibleObjects()
    {
        RaycastHit hit;
        Agent.RaycastMoveDirection(CheckDistance, out hit, DestructibleLayers);

        if (hit.collider.TryGetComponent<DestrcutibleObject>(out DestrcutibleObject d))
        {
            if (d.Health > 0)
            {
                Agent.canMove = false;
                //Agent.enabled = false;
                obj = d;
                return true;
            }
            else
            {
                demon.DemonInMap = true;
            }
        }


        //if(length > 1)
        //{
        //    if (Physics.Raycast(corners[0], (corners[1] - corners[0]).normalized, out RaycastHit hit, CheckDistance, DestructibleLayers))
        //    {
        //        if(hit.collider.TryGetComponent<DestrcutibleObject>(out DestrcutibleObject d))
        //        {
        //            if(d.Health > 0)
        //            {
        //                OrigianlPath = Agent.path;
        //                Agent.enabled = false;
        //                obj = d;
        //                return true;
        //            }
        //            else
        //            {
        //                demon.DemonInMap = true;
        //            }
        //        }
        //    }
        //}

        return false;
    }
}
