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
    [SerializeField] float hitForce = 100f;
    [SerializeField] AnimationCurve damageCurve;
    [SerializeField] AnimationCurve distanceChargeCurve;
    [SerializeField,Range(0,1)] float minChargePercent;
    [SerializeField] float hitCheckRadius;
    [SerializeField] LayerMask enemyLayer, wallLayer;
    
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
                caster.animator.SetTrigger("Cast");
				caster.animator.SetBool("Held", false);
				break;
                
            case State.Charging:
                if (direction != Vector3.up)
                {
                    caster.animator.SetBool("Held", true);
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
	List<Health> healths = new List<Health>();
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
                    if(charge< minChargePercent)
                    {
						chargeTimer = 0;
						state = State.None;
						stats.speedMulti += chargeMoveSpeedModifier;
					}
                    else
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

                Collider[] colliders = Physics.OverlapSphere(caster.castOrigin.position, hitCheckRadius, enemyLayer);
				
				foreach (Collider collider in colliders)
                {
                    if(collider.TryGetComponent(out HitBox hitBox))
                    {
                        if (!healths.Contains(hitBox.health))
                        {
                            if (hitBox.health.TryGetComponent(out Rigidbody rigidbody))
                            {
                                Vector3 forceVector = -(rigidbody.position - caster.castOrigin.position);
                                forceVector.y = 0;
                                forceVector = forceVector.normalized;// * (1/forceVector.sqrMagnitude);
                                rigidbody.AddForce(forceVector * hitForce);

                            }
                            hitBox.OnHit(damageCurve.Evaluate(chargeTimer/flightTime), HitType.ABILITY);
                            healths.Add(hitBox.health);
                        }
                    }
                }



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
        caster.animator.SetBool("Held", false);
        state = State.Release;
        healths.Clear();
        flightTime = distanceChargeCurve.Evaluate(chargeTimer / maxChargeTime) / chargeSpeed;
        
        caster.RemoveBlood(bloodCost);
		chargeTimer = 0;
	}

    
}
