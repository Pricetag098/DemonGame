using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Perks/AoeDamage")]
public class AoeDamagePerk : Perk
{
	[SerializeField] float decayRate;
	[SerializeField] float damage;
	[SerializeField] float radius;
	[SerializeField] float upgradedRadius;
	[SerializeField] float triggerPoint;
	[SerializeField] float damageCount;
	[SerializeField] LayerMask enemyLayer;
	
	protected override void OnEquip()
	{
		manager.tickPerk += Tick;
		manager.GetComponentInChildren<Holster>().OnDealDamage += AddDamage;
	}
	
	void Tick()
	{
		damageCount -=Time.deltaTime;
		if(damageCount < 0)
			damageCount = 0;
	}
	void AddDamage(float amount)
	{
		damageCount += amount;
		if(damageCount > triggerPoint)
		{
			DealDamage();
			damageCount = 0;
		}
	}

	void DealDamage()
	{
		List<Health> healths = new List<Health>();
		Collider[] hits = Physics.OverlapSphere(manager.transform.position, upgraded? upgradedRadius : radius,enemyLayer);
		for(int i = 0; i < hits.Length; i++)
		{
			HitBox hitBox;
			if(hits[i].TryGetComponent(out hitBox))
			{
				if(!healths.Contains(hitBox.health))
					healths.Add(hitBox.health);
			}
		}
		for(int i = 0; i < healths.Count; i++)
		{
			Health health = healths[i];
			health.TakeDmg(damage);
		}
	}

	protected override void OnUnEquip()
	{
		manager.tickPerk -= Tick;
		manager.GetComponentInChildren<Holster>().OnDealDamage -= AddDamage;
	}
}
