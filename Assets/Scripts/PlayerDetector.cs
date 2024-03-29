using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDetector : MonoBehaviour
{
    public UnityEvent enter;
    public UnityEvent exit;
	private bool check = true;

	private void OnTriggerStay(Collider other)
	{
		if (check)
		{
			enter.Invoke();
			check = false;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		exit.Invoke();
		check = true;
	}
}
