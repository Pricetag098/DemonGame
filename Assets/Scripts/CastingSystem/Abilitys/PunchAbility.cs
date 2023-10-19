using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "abilities/Punch")]
public class PunchAbility : Ability
{
    public enum State
    {
        None,
        Charging,
        Release
    }
    [SerializeField] State state;
    PlayerStats stats;
    Rigidbody rb;
    float chargeTimer = 0;
    public float maxChargeTime = 3;
    [SerializeField] float chargeSpeed = 30;
    [SerializeField] AnimationCurve distanceChargeCurve;
    [SerializeField,Range(0,1)] float minChargePercent;
    [Tooltip("Subtracts from the moveSpeedModifier in player stats")]
    [SerializeField, Range(0, 1)] float chargeMoveSpeedModifier;
    float flightTime = 0;
	protected override void OnEquip()
	{
		stats = caster.GetComponent<PlayerStats>();
        rb = caster.GetComponent<Rigidbody>();
	}
    bool pressedThisFrame;
    Vector3 aimDir;
	public override void Cast(Vector3 origin, Vector3 direction)
	{
        
		switch(state)
        {
            case State.None:
                state = State.Charging;
                stats.speedMulti -= chargeMoveSpeedModifier;
                break;
                
            case State.Charging:
                if (direction != Vector3.up)
                {
					aimDir = direction;
                    aimDir.y = 0;
                    aimDir.Normalize();
				}
				
				break;
            case State.Release:
                break;
        }
        pressedThisFrame = true;
	}

	public override void Tick()
	{
		switch (state)
		{
			case State.None:
				break;
			case State.Charging:
                float charge = chargeTimer / maxChargeTime;

				if (!pressedThisFrame ||  charge > 1)
                {

                    Release();
                }
                else
                {
                    chargeTimer += Time.deltaTime;
                }
				break;
			case State.Release:
                rb.velocity = aimDir * chargeSpeed;
                chargeTimer += Time.deltaTime;
                if(chargeTimer > flightTime)
                {
                    chargeTimer = 0;
                    state = State.None;
                    stats.speedMulti += chargeMoveSpeedModifier;
                }
				break;
		}

        pressedThisFrame =  false;
	}
    void Release()
    {
        state = State.Release;
        
        flightTime = distanceChargeCurve.Evaluate(chargeTimer / maxChargeTime) / chargeSpeed;

		chargeTimer = 0;
	}

    
}
