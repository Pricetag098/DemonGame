using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

[CreateAssetMenu(menuName = "abilities/Sword")]
public class SwordAbility : Ability
{
	[SerializeField] LayerMask layers;
	[SerializeField] float range;
	[SerializeField] float rad;
	[SerializeField] VfxSpawnRequest vfx;
	[SerializeField] float swingsPerMin;
	[SerializeField] float damage;
	[SerializeField] int points;
	[SerializeField] VfxSpawnRequest slashVfx;
	float cooldown;
	float timer;
	Trail trail;
    [SerializeField] float swingDamageDelay = .1f;

	bool inputBuffered;
    int combo =0, comboLength = 3;
    [SerializeField, Range(0, 1)] float inputBufferPoint;
    bool didDamage;
	protected override void OnEquip()
	{
		cooldown = 1 / (swingsPerMin / 60);
        timer = cooldown;
		trail = caster.animator.transform.parent.parent.GetComponentInChildren<Trail>();
        didDamage = true;
		
	}


	public override void Cast(Vector3 origin, Vector3 direction)
	{
		if(timer / cooldown > inputBufferPoint && !inputBuffered && combo < comboLength)
        {
            inputBuffered = true;
            combo++;
        }
        
    }
    bool swung = false;

    public override void Tick(Vector3 origin, Vector3 direction)
	{
		timer += Time.deltaTime;

        if(timer > swingDamageDelay && !didDamage)
        {
            didDamage = true;
            DoSwing(origin, direction);
        }


        if(timer > cooldown)
        {
            if (inputBuffered)
            {
                swung = true;
                timer = 0;
                inputBuffered = false;
                didDamage = false;

                caster.animator.SetTrigger("Cast");
            }
            else
            {
                if(swung)
                {
                    swung = false;
                    combo = 0;
                    caster.animator.SetTrigger("Exit");
                }
                
            }
        }
		
		
	}

    void DoSwing(Vector3 origin, Vector3 direction)
    {
        Debug.Log("Swing");
        List<Health> healths = new List<Health>();
        //slashVfx.Play(origin, direction);
        caster.RemoveBlood(bloodCost);
        RaycastHit[] hits = Physics.SphereCastAll(origin, rad, direction, range, layers);
        foreach (RaycastHit hit in hits)
        {
            HitBox hb;
            if (hit.collider.TryGetComponent(out hb))
            {
                if (healths.Contains(hb.health))
                    continue;
                healths.Add(hb.health);
                OnHit(hb.health);
                if (hb.health.TakeDmg(damage * caster.DamageMulti, HitType.ABILITY))
                    caster.OnKill();
                if (hit.point == Vector3.zero)
                {
                    Vector3 pos = hit.collider.ClosestPoint(origin);
                    vfx.Play(pos, pos - origin);
                }
                else
                {
                    vfx.Play(hit.point, hit.normal);
                }

            }
        }
    }
	public override void OnHit(Health health)
	{
        if (health.dead)
            return;
        if (caster.playerStats.Enabled)
			caster.playerStats.Value.GainPoints(points);
	}

    public override void StartSelect()
    {
		//Enable sword
        //Animate sword in
    }
    public override void EndSelect()
    {
        //enable trail
        swung = false;
        combo = 0;
        inputBuffered = false;
        trail.GetComponent<MeshRenderer>().enabled = true;
    }

    public override void StartDeSelect()
    {
        //Animate sword out
        //disable trail
        Debug.Log("Deselct");
        trail.GetComponent<MeshRenderer>().enabled = false;
    }

    public override void EndDeSelect()
    {
        //Disable Sword
		
    }
}
