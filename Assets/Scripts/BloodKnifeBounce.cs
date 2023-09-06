using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodKnifeBounce : MonoBehaviour
{
	DamageProjectiles damageProjectiles;
	[SerializeField] AimAssist aimAssist;
	private void Awake()
	{
		damageProjectiles = GetComponent<DamageProjectiles>();
		damageProjectiles.onPenetrate += Bounce;
	}
	void Bounce()
	{

	}
}
