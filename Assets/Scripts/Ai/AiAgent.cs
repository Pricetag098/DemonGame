using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    AiAgent[] others = new AiAgent[0];
    
    public float followSpeed;
    public float acceleration;
    
    public LayerMask wallLayers;
    public float scanRadius;
    public int scanRays;
    public float groundingRange, groundingRadius;
    public float dispersionForce;
    public float gravityScale;
    public float indexChangeDistance = .1f;
    public float stopingDistance = 1;
    Rigidbody rb;
    int pathIndex = 1;
	float radius;
	public AgentPath path = new AgentPath();
    public bool canMove = true;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        others = FindObjectsOfType<AiAgent>();
        rb.sleepThreshold = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.forward = rb.velocity;
    }

    private void FixedUpdate()
    {
        Vector3 idealVel;

		
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, groundingRange,wallLayers))
        {
			if (canMove && path.hasPath)
			{
				UpdateRadius();
				
				Vector3 toTarget = (path.path[pathIndex] - transform.position).normalized;
				idealVel = toTarget * followSpeed;
                if(hit.normal != Vector3.zero)
                idealVel = Vector3.ProjectOnPlane(idealVel, hit.normal);
			}
			else
			{
				idealVel = Vector3.zero;
			}
			rb.AddForce(GetPushForce() * dispersionForce);
			Vector3 turningVel = idealVel - rb.velocity;
			rb.AddForce(turningVel * acceleration);
            if(pathIndex == path.pathLength - 1)
            {
                if (Vector3.Distance(transform.position, path[pathIndex]) < stopingDistance)
                {
                    path.hasPath = false;
                }
            }
			if (Vector3.Distance(transform.position, path[pathIndex]) < indexChangeDistance)
			{
				pathIndex++;
				if (pathIndex >= path.pathLength)
				{
					pathIndex = path.pathLength - 1;
				}
			}
		}
        else
        {
            rb.AddForce(Vector3.down * gravityScale);
        }
        
        
    }

    public void UpdatePath(Transform target)
    {
        CalculatePath(transform.position, target.position, path);
		pathIndex = 1;
	}

    

    void UpdateRadius()
    {
        radius = scanRadius;
        float angle = 360 / scanRays;
        for(int i = 0; i < scanRays; i++)
        {
            if(Physics.Raycast(transform.position,Quaternion.Euler(transform.up * angle*i) * transform.forward,out RaycastHit hit, scanRadius, wallLayers))
            {
                if(hit.distance < radius)
                {
                    radius = hit.distance;
                }
            }
        }
        //radius = Mathf.Min(radius, Vector3.Distance(transform.position, path[pathIndex]));
    }

    Vector3 GetPushForce()
    {
        Vector3 force = Vector3.zero;
        foreach(AiAgent other in others)
        {
            if (other == this)
                continue;
            Vector3 dirTo = transform.position - other.transform.position;
            float dist = dirTo.magnitude;
            if (dist == 0)
                dist = .000001f;
            dirTo /= dist;

            force += dirTo * SmoothingVal(radius, dist);

        }

        return force;   


    }
	float SmoothingVal(float radius, float distance)
	{
		float val = Mathf.Max(0, radius - distance);
		return val * val * val;
	}
	[System.Serializable]
    public class AgentPath
    {
        public int pathLength;
        public Vector3[] path;
        public bool hasPath = false;
        public Vector3 this[int key]
        {
            get { return path[key]; }
        }

        public AgentPath()
        {
            pathLength = 0;
            path = new Vector3[100];
        }
    }

	void CalculatePath(Vector3 start, Vector3 end,AgentPath agentPath)
	{
        NavMeshPath path = new NavMeshPath();
        
		agentPath.hasPath = NavMesh.CalculatePath(start, end, NavMesh.AllAreas,path);
		agentPath.pathLength = path.GetCornersNonAlloc(agentPath.path);
        
	}
	private void OnDrawGizmos()
    {
		Gizmos.color = Color.white;
		if (others.Length > 0)
        Gizmos.DrawRay(transform.position, GetPushForce());
        Gizmos.color = Color.magenta;
        if(path.pathLength >0)
        {
            for(int i = 0;i< path.pathLength;i++)
            {
                Gizmos.DrawWireSphere(path.path[i], indexChangeDistance);
            }
        }

        Gizmos.color = Color.blue;
		float angle = 360 / scanRays;
		for (int i = 0; i < scanRays; i++)
		{
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, angle * i, 0) * transform.forward * scanRadius);
		}

	}

}
