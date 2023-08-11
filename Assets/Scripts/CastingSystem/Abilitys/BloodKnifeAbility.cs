using UnityEngine;

[CreateAssetMenu(menuName = "abilities/Ranged")]
public class BloodKnifeAbility : Ability
{
    [SerializeField] protected GameObject prefab;
    protected ObjectPooler projectileSpawner;

    protected float chargeTime;
    [SerializeField] protected float maxChargeTime;
    [SerializeField] protected float minChargeTime;
    bool held;

    [SerializeField] AnimationCurve chargeDamageCurve = AnimationCurve.Linear(0, 0, 1, 100);
    [SerializeField] AnimationCurve chargeVelocityCurve = AnimationCurve.Linear(0, 0, 1, 50);
    [SerializeField] AnimationCurve chargePenetrationCurve = AnimationCurve.Linear(0, 0, 1, 3);
    [SerializeField] AnimationCurve chargeCostCurve = AnimationCurve.Linear(0, 10, 1, 100);



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
        }
        chargeTime = Mathf.Clamp(chargeTime + Time.deltaTime, 0, maxChargeTime);
        lastAimDir = direction;
        lastOrigin = origin;

        held = true;
    }

    public override void Tick()
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
        Vector3 velocity = speed * lastAimDir;
        projectileSpawner.Spawn().GetComponent<DamageProjectiles>().Shoot(lastOrigin, velocity, damage,caster.castOrigin,Mathf.RoundToInt(chargePenetrationCurve.Evaluate(chargePercent)));
        caster.blood -= chargeCostCurve.Evaluate(chargePercent);
    }

   
}