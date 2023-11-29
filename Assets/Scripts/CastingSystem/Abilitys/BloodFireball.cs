using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="abilities/fireball")]
public class BloodFireball : Ability
{
    [SerializeField] protected GameObject prefab;
    protected ObjectPooler projectileSpawner;

    [SerializeField]protected float chargeTime;
    [SerializeField] protected float maxChargeTime;
    [SerializeField] protected float minChargeTime;
    bool held;
    [SerializeField] int points;
    [SerializeField] AnimationCurve chargeDamageCurve = AnimationCurve.Linear(0, 0, 1, 100);
    [SerializeField] AnimationCurve chargeVelocityCurve = AnimationCurve.Linear(0, 0, 1, 50);
    [SerializeField] AnimationCurve chargeRadiusCurve = AnimationCurve.Linear(0, 5, 1, 10);
    [SerializeField] VfxSpawnRequest shootFx;

    protected Vector3 lastAimDir, lastOrigin;
    

    bool startedCasting;
    protected override void OnEquip()
    {
        projectileSpawner = new GameObject().AddComponent<ObjectPooler>();
        projectileSpawner.CreatePool(prefab, 10);
    }

    protected override void OnDeEquip()
    {
        Destroy(projectileSpawner.gameObject);
    }
    public override void Cast(Vector3 origin, Vector3 direction)
    {
        if (!startedCasting)
        {
            startedCasting = true;
			caster.animator.SetTrigger("Cast");
			caster.animator.SetBool("Held", true);
		}
        chargeTime = Mathf.Clamp(chargeTime + Time.deltaTime, 0, maxChargeTime);
        lastAimDir = direction;
        lastOrigin = origin;
        held = true;
    }

    public override void Tick(Vector3 origin, Vector3 direction)
    {
        if (!held && chargeTime > 0)
        {

            if (chargeTime > minChargeTime)
                Launch();
			

			startedCasting = false;
            chargeTime = 0;
        }


        held = false;
    }

    protected virtual void Launch()
    {
        float chargePercent = chargeTime / maxChargeTime;
        float damage = chargeDamageCurve.Evaluate(chargePercent);
        float speed = chargeVelocityCurve.Evaluate(chargePercent);
        float radius = chargeRadiusCurve.Evaluate(chargePercent);
        Vector3 velocity = speed * lastAimDir;
        caster.RemoveBlood(bloodCost);
        projectileSpawner.Spawn().GetComponent<Fireball>().Shoot(lastOrigin, velocity, damage * caster.DamageMulti, this, radius);
        shootFx.Play(caster.castOrigin.position, lastAimDir);
		caster.animator.SetBool("Held", false);
	}

    public override void OnHit(Health health)
	{
        if (caster.playerStats.Enabled)
            caster.playerStats.Value.GainPoints(points);
	}

    
}
