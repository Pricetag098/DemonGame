using BlakesSpatialHash;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : SpatialHashObject
{
    public float followSpeed;
    public float acceleration;
    public float rotationSpeed;
    public bool canRotate;
    public float RemainingDistancePath;
    
    public LayerMask wallLayers;
    public float scanRadius;
    public int scanRays;
    public float groundingRange, groundingRadius;
    public float dispersionForce;
    public float gravityScale;
    public float indexChangeDistance = .1f;
    public float stopingDistance = 1;
    private Rigidbody rb;
    public int pathIndex = 1;
	float radius;
    public float rayHeightOffset;
	public AgentPath path = new AgentPath();
    public bool canMove = true;

    private Quaternion lastRotation = Quaternion.identity;
    private Vector3 upVector = Vector3.zero;
    private DemonFramework demon;

    bool initalise = false;

    public void Initalised()
    {
        if(initalise == false)
        {
            Initalise();
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.sleepThreshold = 0;
        demon = GetComponent<DemonFramework>();
    }

    // Update is called once per frame
    void Update()
    {
        RemainingDistancePath = RemainingDistance;

        if (Vector3.Distance(transform.position, path[pathIndex]) < indexChangeDistance)
        {
            pathIndex++;
            if (pathIndex >= path.pathLength)
            {
                pathIndex = path.pathLength - 1;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 idealVel = Vector3.zero;

        if (Physics.Raycast(transform.position + transform.up * rayHeightOffset, -transform.up, out RaycastHit hit, groundingRange, wallLayers))
        {
            if (canMove && path.hasPath)
			{
				UpdateRadius();
				
				Vector3 toTarget = (path.corners[pathIndex] - transform.position).normalized;
				idealVel = toTarget * followSpeed;
                if(hit.normal != Vector3.zero)
                idealVel = Vector3.ProjectOnPlane(idealVel, hit.normal);
                upVector = hit.normal;
			}

			rb.AddForce(GetPushForce() * dispersionForce * Time.fixedDeltaTime);

			Vector3 turningVel = idealVel - rb.velocity;
			rb.AddForce(turningVel * acceleration * Time.fixedDeltaTime);
		}
        else
        {
            rb.AddForce(Vector3.down * gravityScale * Time.fixedDeltaTime);
        }
    }

    public bool UpdatePath(Transform target)
    {
        NavMeshPath Navpath = new NavMeshPath();

        if(CalculatePath(target.position, Navpath))
        {
            SetPath(Navpath);
        }

        if (RemainingDistancePath < stopingDistance)
        {
            path.hasPath = false;
            return false;
        }

        return true;
	}

    public void LookDirection()
    {
        if(canRotate == true)
        {
            Vector3 forward = transform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 targetDirection = path[pathIndex] - transform.position;
            targetDirection.y = 0;
            targetDirection.Normalize();
            

            Quaternion newRotation = Quaternion.LookRotation(targetDirection);

            Quaternion rot = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);

            lastRotation = rot;
            transform.rotation = rot;
        }
        else
        {
            transform.rotation = lastRotation;
        }
    }

    
    void UpdateRadius()
    {
        radius = scanRadius;
        float angle = 360 / scanRays;
        for(int i = 0; i < scanRays; i++)
        {
            if(Physics.Raycast(transform.position + transform.up * rayHeightOffset, Vector3.ProjectOnPlane(Quaternion.Euler(Vector3.up * angle*i) * transform.forward,transform.up),out RaycastHit hit, scanRadius, wallLayers))
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

        if(demon.GetDemonInMap == false) return force;

        foreach(SpatialHashObject other in Objects)
        {
            if (other == this) { continue; }
                
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
        public Vector3[] corners;
        public bool hasPath = false;
        public Vector3 this[int key]
        {
            get { return corners[key]; }
        }

        public AgentPath()
        {
            pathLength = 0;
            corners = new Vector3[100];
        }

        public void ResetPath()
        {
            hasPath = false;
            pathLength = 0;
            corners = new Vector3[100];
        }
    }
    public float RemainingDistance
    {
        get
        {
            float num = 0;

            if (path.pathLength == 2) // if start and end position (only 2 positions) return distance to next index(end)
            {
                num = Vector3.Distance(path[1], transform.position);

                return num; 
            }

            num += Vector3.Distance(path[pathIndex], transform.position); // add current distance to next index

            for (int i = pathIndex; i < path.pathLength - 1; i++) // loop over remaining positions and add to total
            {
                num += Vector3.Distance(path[i], path[i + 1]);
                
            }

            return num;
        }
    }

    public float VelocityMag
    {
        get
        {
            return rb.velocity.magnitude;
        }
    }

    public bool CalculatePath(Vector3 start, Vector3 end, NavMeshPath path)
    {
        return NavMesh.CalculatePath(start, end, NavMesh.AllAreas, path);
    }
    public bool CalculatePath(Vector3 end, NavMeshPath path)
    {
        return NavMesh.CalculatePath(transform.position, end, NavMesh.AllAreas, path);
    }

    public void SetPath(NavMeshPath navPath)
    {
        path.pathLength = navPath.GetCornersNonAlloc(path.corners);

        if (path.pathLength > 1)
        {
            path.hasPath = true;
            pathIndex = 1;
        }
    }

	void CalculatePath(Vector3 start, Vector3 end, AgentPath agentPath)
	{
        NavMeshPath path = new NavMeshPath();
        
		agentPath.hasPath = NavMesh.CalculatePath(start, end, NavMesh.AllAreas,path);
		agentPath.pathLength = path.GetCornersNonAlloc(agentPath.corners);
	}

    #region HelperFunctions
    public void RaycastMoveDirection(float distance, out RaycastHit hit, LayerMask layer)
    {
        Physics.Raycast(transform.position, rb.velocity, out RaycastHit obj, distance, layer);
        hit = obj;
    }

    public void RemoveFromSpatialHash()
    {
        Grid.cells.Remove(this);
    }

    public void AddToSpatialHash()
    {
        Grid.cells.Insert(this);
    }
    public void SetNearbyAgents(List<SpatialHashObject> objs)
    {
        Objects = objs;
    }

    public void SetFollowSpeed(float num)
    {
        followSpeed = num;
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.white;
        //if (Objects.Count > 0)
        //      Gizmos.DrawRay(transform.position, GetPushForce());
        //      Gizmos.color = Color.magenta;
        if (path.pathLength > 0)
        {
            for (int i = 0; i < path.pathLength; i++)
            {
                Gizmos.DrawWireSphere(path.corners[i], indexChangeDistance);
            }
        }

        //      Gizmos.color = Color.blue;
        //float angle = 360 / scanRays;
        //for (int i = 0; i < scanRays; i++)
        //{
        //          Gizmos.DrawRay(transform.position + transform.up * rayHeightOffset, Vector3.ProjectOnPlane(Quaternion.Euler(Vector3.up * angle * i) * transform.forward, transform.up) * scanRadius);
        //}

    }

}
