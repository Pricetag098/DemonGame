using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] float minHeight;

	private void Update()
	{
		if(health.transform.position.y < minHeight && Time.timeScale > 0)
		{
			health.TakeDmg(float.PositiveInfinity);
		}
	}
}
