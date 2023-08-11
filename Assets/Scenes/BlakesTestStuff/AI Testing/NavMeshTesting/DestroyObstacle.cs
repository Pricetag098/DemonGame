using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DestroyObstacle : MonoBehaviour
{
    [SerializeField] private int DestructibleCheckRate = 10;
    [SerializeField] private float CheckDistance = 1f;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private DemonBase demon;
    [SerializeField] private float AttackDelay = 1f;
    [SerializeField] private int DestructibleAttackDamage = 1;
    [SerializeField] private LayerMask DestructibleLayers;

    private NavMeshPath OrigianlPath;

    private bool checkForObjects;
    private bool foundObject;
    private DestrcutibleObject obj;
    private float timer;

    public void Detection(int amount)
    {
        timer += Time.deltaTime;

        if(amount % DestructibleCheckRate == 0 && foundObject == false)
        {
            checkForObjects = CheckForDistructibleObjects();
            amount = 0;
        }

        if (checkForObjects == false && foundObject == false) return;

        foundObject = true;

        if (HelperFuntions.TimerGreaterThan(timer, AttackDelay))
        {
            if(obj != null)
            {
                obj.TakeDamage(DestructibleAttackDamage);
                demon.PlayAnimation("Attack");

                if (obj.Health <= 0)
                {
                    Agent.enabled = true;
                    Agent.path = OrigianlPath;
                    obj = null;
                }
                timer = 0;
                amount = 0;
            }
        }
    }

    private bool CheckForDistructibleObjects()
    {
        Vector3[] corners = new Vector3[2];

        int length = Agent.path.GetCornersNonAlloc(corners);
        if(length > 1)
        {
            if (Physics.Raycast(corners[0], (corners[1] - corners[0]).normalized, out RaycastHit hit, CheckDistance, DestructibleLayers))
            {
                if(hit.collider.TryGetComponent<DestrcutibleObject>(out DestrcutibleObject d))
                {
                    OrigianlPath = Agent.path;
                    Agent.enabled = false;
                    obj = d;
                    return true;
                }
            }
        }

        return false;
    }
}
