using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TestAgentThing : MonoBehaviour
{
	TestAgentThing[] others;
	public LayerMask wallLayers;
	public float scanRadius;
	public float dispersionForce;
	public int scanRays;
	float radius;
	NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
		others = FindObjectsOfType<TestAgentThing>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void FixedUpdate()
	{
		UpdateRadius();
		agent.velocity += GetPushForce() * dispersionForce * Time.fixedDeltaTime;
		agent.velocity += -transform.right * Vector3.Dot(agent.velocity, transform.right);
	}

	Vector3 GetPushForce()
	{
		Vector3 force = Vector3.zero;
		foreach (TestAgentThing other in others)
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

	void UpdateRadius()
	{
		radius = scanRadius;
		float angle = 360 / scanRays;
		for (int i = 0; i < scanRays; i++)
		{
			if (Physics.Raycast(transform.position, Quaternion.Euler(0, angle * i, 0) * transform.forward, out RaycastHit hit, scanRadius, wallLayers))
			{
				if (hit.distance < radius)
				{
					radius = hit.distance;
				}
			}
		}
		//radius = Mathf.Min(radius, Vector3.Distance(transform.position, path[pathIndex]));
	}
}
