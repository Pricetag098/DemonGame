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
    [SerializeField] float minDistance = 0;
    [Range(-1, 1)]
    [SerializeField] float minDirectionValue = -1;
    [Range(0f, 1f)]
    [SerializeField] float assistWeight = 1;
    public bool GetAssistedAimDir(Vector3 aimDir,Vector3 origin, float projectileSpeed,out Transform bestTarget, List<Health> ignoreList)
    {
        bestTarget = null;
        if (assistWeight == 0)
		{
            return false;
        }
            

        Collider[] colliders = Physics.OverlapSphere(origin, maxRange, aimAssistLayer);
        List<Health> healths = new List<Health>();
        
        bool foundTarget = false;
        float bestVal = float.NegativeInfinity;
        for (int i = 0; i < colliders.Length; i++)
        {
            
            HitBox hb;
            if (colliders[i].TryGetComponent(out hb))
            {
                if (healths.Contains(hb.health) || ignoreList.Contains(hb.health))
                    continue;
                if(hb.health.TryGetComponent(out VfxTargets target))
				{
                    
                    healths.Add(hb.health);
                    Vector3 casterToTarget = target.core.position - origin;
                    

                    //float time = casterToTarget.magnitude / projectileSpeed;
                    //Vector3 newPos = target.core.position + target.GetVelocity() * time;
                    //casterToTarget = newPos - origin;
                    //casterToTarget.y = 0;

                    Vector3 casterToTargetNorm = casterToTarget.normalized;
                    float directionValue = Vector3.Dot(aimDir, casterToTargetNorm);
                    float distance = casterToTarget.magnitude;
                    if (directionValue < minDirectionValue || distance < minDistance)
                        continue;
                    directionValue *= directionWeight;
                    float val = directionValue + (1 / distance) * distanceWeight;
                    if (val > bestVal)
                    {
                        bestVal = val;
                        bestTarget = target.core;
                        foundTarget = true;
                    }
                }
                

            }
        }

        return foundTarget;//Vector3.Lerp(aimDir, bestTarget, assistWeight).normalized;
    }

}
