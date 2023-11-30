using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RayTester : MonoBehaviour
{
    public int maxDepth;
    public float maxRange;
    public float stepOffset;
    public LayerMask layers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDrawGizmos()
    {
        Vector3 test = transform.position + transform.forward * maxRange;
        RecursiveRaycast(transform.position, transform.forward, 0,0,ref test);
        Gizmos.DrawLine(transform.position, test);
    }


    void RecursiveRaycast(Vector3 point,Vector3 dir,int depth,float range,ref Vector3 endPoint)
    {
        if (depth > maxDepth)
            return;
        if (Physics.Raycast(point, dir, out RaycastHit hit, maxRange - range,layers))
        {
            Gizmos.DrawLine(point, hit.point);
            Gizmos.DrawWireSphere(hit.point, .1f);
            endPoint = hit.point;
            RecursiveRaycast(hit.point + dir * stepOffset, dir, depth + 1, range + hit.distance, ref endPoint);

        }
        else
            return;
        
    }

}
