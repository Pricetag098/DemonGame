using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessingStatusHandler : MonoBehaviour
{
	public List<Blessing> activeBlessings = new List<Blessing>();
	[HideInInspector]public Holster holster;
	[HideInInspector]public Spawner spawner;
	[HideInInspector]public PlayerStats playerStats;
	[HideInInspector]public DestrcutibleObject[] destrcutibleObjects;
	[HideInInspector] public AbilityCaster abilityCaster;

	private void Awake()
	{
		destrcutibleObjects= FindObjectsOfType<DestrcutibleObject>();
		holster = GetComponentInChildren<Holster>();
		spawner = FindObjectOfType<Spawner>();
		playerStats = GetComponent<PlayerStats>();
		abilityCaster = GetComponent<AbilityCaster>();
	}

	private void Update()
	{
		for(int i = activeBlessings.Count - 1; i >= 0; i--)
		{
			activeBlessings[i].Tick();
		}
	}
}
