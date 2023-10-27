using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidExperiment : MonoBehaviour
{
    [SerializeField] Transform target;
	BoidExperiment[] boids;

	public Vector3 direction;
	private void Awake()
	{
		boids = FindObjectsOfType<BoidExperiment>();
		direction = Random.insideUnitSphere;
		direction.y = 0;
	}

	private void Update()
	{
		Vector3 dir = Vector3.zero;
		foreach(BoidExperiment boid in boids)
		{
			dir += boid.direction;
		}
		dir /= boids.Length;
		direction += dir * Time.deltaTime;
		transform.position = direction * Time.deltaTime;
	}
}
