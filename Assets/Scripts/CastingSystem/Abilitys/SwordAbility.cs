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

	bool inputBuffered;

	protected override void OnEquip()
	{
		cooldown = 1 / (swingsPerMin / 60);

		trail = caster.animator.transform.parent.parent.GetComponentInChildren<Trail>();
		
	}


	public override void Cast(Vector3 origin, Vector3 direction)
	{
		
        
    }
   

    public override void Tick(Vector3 origin, Vector3 direction)
	{
		timer += Time.deltaTime;

		if(inputBuffered)
		{
            timer = 0;
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
                    hb.health.TakeDmg(damage * caster.DamageMulti, HitType.ABILITY);
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
            caster.animator.ResetTrigger("Cast");
            caster.animator.SetTrigger("Cast");
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
		trail.GetComponent<MeshRenderer>().enabled = true;
    }

    public override void StartDeSelect()
    {
        //Animate sword out
        //disable trail
        trail.GetComponent<MeshRenderer>().enabled = false;
    }

    public override void EndDeSelect()
    {
        //Disable Sword
		
    }
}
