using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    [System.Flags]
    public enum DispersionMask
    {
        Ignore = 1,
        Ground = 2,
        Flying = 4
    }

    public enum DispersionLayer
    {
        Ignore = 0,
        Ground = 1,
        Flying = 2
    }

    public float followSpeed;
    public float acceleration;
    public float rotationSpeed;
    public bool canRotate;
    public float RemainingDistancePath;
    public Transform GroundedPosition;

    public DispersionLayer dispersionLayer;
    public DispersionMask dispersionMask;
    public LayerMask wallLayers;
    public float scanRadius;
    public int scanRays;
    public float groundingRange;
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
    public int drawIndex = 0;
    public bool grounded = false;

    private Quaternion lastRotation = Quaternion.identity;
    private DemonFramework demon;

    public Vector3 GetPosition { get { return transform.position; } }
    public Vector3 GetLastPosition { get; set; }
    public uint Index { get; set; }
    public bool Enabled { get { return gameObject.activeSelf; } }
    public List<AiAgent> Objects { get; set; }

    [HideInInspector] public SpatialHashGrid3D Grid;

    [Header("Unstuck stats")]
    [SerializeField] private float despawnCheckCooldown;
    [Tooltip("How far the AI needs to move to reset the despawn timer")]
    [SerializeField] private float despawnDistance;
    private Vector3 lastUnstuckPosition;
    private float despawnTimer;


    public void Initalise()
    {
        Grid = SpatialHashGrid3D.Instance;

        Objects = new List<AiAgent>();
    }

    bool initalise = false;

    public void Initalised()
    {
        if(initalise == false)
        {
            initalise = true;
            Initalise();
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.sleepThreshold = 0;
        demon = GetComponent<DemonFramework>();
    }

    void Update()
    {
        RemainingDistancePath = RemainingDistance;

        if (Time.timeScale == 0.0f) { rb.isKinematic = true; }
        else { rb.isKinematic = false; }
    }

    private void FixedUpdate()
    {
        if(Time.timeScale == 0.0f || Time.fixedDeltaTime == 0.0f) { return; }

        Vector3 idealVel = Vector3.zero;



   //     if (Physics.Raycast(transform.position + transform.up * rayHeightOffset, -transform.up, out RaycastHit hit, groundingRange, wallLayers))
   //     {
   //         if (canMove && path.hasPath)
			//{
			//	UpdateRadius();
				
			//	Vector3 toTarget = (path.corners[pathIndex] - transform.position).normalized;
			//	idealVel = toTarget * followSpeed;
   //             if(hit.normal != Vector3.zero)
   //             idealVel = Vector3.ProjectOnPlane(idealVel, hit.normal);
   //             //upVector = hit.normal;
			//}

			//rb.AddForce(GetPushForce() * dispersionForce * Time.fixedDeltaTime);

			//Vector3 turningVel = idealVel - rb.velocity;
			//rb.AddForce(turningVel * acceleration * Time.fixedDeltaTime);

   //         if (Vector3.Distance(transform.position, path[pathIndex]) < indexChangeDistance)
   //         {
   //             pathIndex++;
   //             if (pathIndex >= path.pathLength)
   //             {
   //                 pathIndex = path.pathLength - 1;
   //             }
   //         }
   //     }
   //     else
   //     {
   //         rb.AddForce(Vector3.down * gravityScale * Time.fixedDeltaTime);
   //     }

        if(isGrounded(out RaycastHit hit))
        {
            if (canMove && path.hasPath)
            {
                UpdateRadius();

                Vector3 toTarget = (path.corners[pathIndex] - transform.position).normalized;
                idealVel = toTarget * followSpeed;
                if (hit.normal != Vector3.zero)
                {
                    idealVel = Vector3.ProjectOnPlane(idealVel, hit.normal);
                }
            }
              
            rb.AddForce(GetPushForce() * dispersionForce * Time.fixedDeltaTime * Time.timeScale);

            Vector3 turningVel = idealVel - rb.velocity;
            rb.AddForce(turningVel * acceleration * Time.fixedDeltaTime * Time.timeScale);

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
            rb.AddForce(Vector3.down * gravityScale * Time.fixedDeltaTime * Time.timeScale);
        }

        //stuck fixing
        if(Time.time - despawnTimer > despawnCheckCooldown)
        {
            //if the ai has moved less than despawnDistance, probably stuck and despawn them
            if (Vector3.Distance(GetPosition, lastUnstuckPosition) < despawnDistance)
            {
                GetComponent<Health>().TakeDmg(9999999, HitType.ABILITY);
            }
            else
            {
                ResetStuckTimer();
            }
            lastUnstuckPosition = transform.position;
        }

    }

    public void ResetStuckTimer()
    {
        despawnTimer = Time.time;
    }

    private bool isGrounded(out RaycastHit hit)
    {
        if (Physics.Raycast(GroundedPosition.position, -transform.up, out RaycastHit cast, groundingRange, wallLayers))
        {
            hit = cast;
            grounded = true;
            return true;
        }

        hit = cast;
        grounded = false;
        return false;
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
    }

    Vector3 GetPushForce()
    {
        Vector3 force = Vector3.zero;

        if(demon.GetDemonInMap == false) return force;

        foreach(AiAgent other in Objects)
        {
            if (other == this || !((int)dispersionMask == ((int)dispersionMask | (1 << (int)other.dispersionLayer)))) { continue; }

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
        NavMesh.SamplePosition(transform.position, out NavMeshHit hitStart, 5, NavMesh.AllAreas);
        NavMesh.SamplePosition(end, out NavMeshHit hitEnd, 5, NavMesh.AllAreas);

        //Vector3 startSampledPosition = hitStart.position;
        //Vector3 endSampledPosition = hitEnd.position;

        return NavMesh.CalculatePath(hitStart.position, hitEnd.position, NavMesh.AllAreas, path);
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

        NavMesh.SamplePosition(start, out NavMeshHit hitStart, 5, NavMesh.AllAreas);
        NavMesh.SamplePosition(end, out NavMeshHit hitEnd, 2, NavMesh.AllAreas);

        Vector3 startSampledPosition = hitStart.position;
        Vector3 endSampledPosition = hitEnd.position;
        
		agentPath.hasPath = NavMesh.CalculatePath(startSampledPosition, endSampledPosition, NavMesh.AllAreas,path);
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
    public void SetNearbyAgents(List<AiAgent> objs)
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
        //    Gizmos.DrawRay(transform.position, GetPushForce());
        //Gizmos.color = Color.magenta;

        //Gizmos.color = Color.red;

        //if (path.pathLength > 0)
        //{
        //for (int i = 0; i < path.pathLength; i++)
        //{
        //    Gizmos.DrawWireSphere(path.corners[i], indexChangeDistance);
        //}

        //Gizmos.DrawWireSphere(path.corners[drawIndex], 1);
        //}

        //Gizmos.color = Color.blue;
        //float angle = 360 / scanRays;
        //for (int i = 0; i < scanRays; i++)
        //{
        //    Gizmos.DrawRay(transform.position + transform.up * rayHeightOffset, Vector3.ProjectOnPlane(Quaternion.Euler(Vector3.up * angle * i) * transform.forward, transform.up) * scanRadius);
        //}

        //Physics.Raycast(transform.position + transform.up * rayHeightOffset, -transform.up, out RaycastHit hit, groundingRange, wallLayers;


        //Vector3 startpos = GroundedPosition.position;
        //Vector3 endpos = GroundedPosition.position + -transform.up * groundingRange;

        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(startpos, endpos);
    }
}

public enum PathingType
{
    Grounded,
    Flying
}