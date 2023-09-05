using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AimAssist
{
    [SerializeField] float maxRange = 100;
    [SerializeField] LayerMask aimAssistLayer  = 512;
    [SerializeField] float directionWeight = 1;
    [SerializeField] float distanceWeight = 1;
    //[SerializeField] float priorityWeight = 1;
    [Range(-1, 1)]
    [SerializeField] float minDirectionValue = -1;
    [Range(0f, 1f)]
    [SerializeField] float assistWeight = 1;
    public bool GetAssistedAimDir(Vector3 aimDir,Vector3 origin, float projectileSpeed,out Transform bestTarget)
    {
        bestTarget = null;
        if (assistWeight == 0)
		{
            
            return false;
        }
            

        Collider[] colliders = Physics.OverlapSphere(origin, maxRange, aimAssistLayer);

        bool foundTarget = false;
        float bestVal = float.NegativeInfinity;
        for (int i = 0; i < colliders.Length; i++)
        {
            HitBox hb;
            if (colliders[i].TryGetComponent(out hb))
            {
                if(hb.health.TryGetComponent(out VfxTargets target))
				{
                    foundTarget = true;

                    Vector3 casterToTarget = target.core.position - origin;
                    casterToTarget.y = 0;

                    //float time = casterToTarget.magnitude / projectileSpeed;
                    //Vector3 newPos = target.core.position + target.GetVelocity() * time;
                    //casterToTarget = newPos - origin;
                    //casterToTarget.y = 0;

                    Vector3 casterToTargetNorm = casterToTarget.normalized;
                    float directionValue = Vector3.Dot(aimDir, casterToTargetNorm) * directionWeight;
                    if (directionValue < minDirectionValue)
                        continue;
                    float val = directionValue + (1 / casterToTarget.magnitude) * distanceWeight;
                    if (val > bestVal)
                    {
                        bestVal = val;
                        bestTarget = target.core;
                    }
                }
                

            }
        }

        return foundTarget;//Vector3.Lerp(aimDir, bestTarget, assistWeight).normalized;
    }

}
