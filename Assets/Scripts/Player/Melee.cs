using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Melee : MonoBehaviour
{
	[SerializeField] LayerMask layers;
	[SerializeField] float range;
	[SerializeField] float rad;
	
	[SerializeField] float swingsPerMin;
	[SerializeField] float damage;
	[SerializeField] int points;
	
	[SerializeField] InputActionProperty input;
	[SerializeField] SoundPlayer soundPlayer;
	PlayerStats playerStats;
	float cooldown;
	float timer;
	[SerializeField] Animator animator;
	[SerializeField] string animationKey = "Melee";

	protected void Awake()
	{
		cooldown = 1 / (swingsPerMin / 60);
		playerStats = GetComponent<PlayerStats>();
		input.action.performed += Cast;
	}
	private void OnEnable()
	{
		input.action.Enable();
	}
	private void OnDisable()
	{
		input.action.Disable();
	}

	public void Cast(InputAction.CallbackContext context)
	{
		Vector3 origin = Camera.main.transform.position;
		Vector3 direction = Camera.main.transform.forward;

		if (timer < cooldown)
			return;
		timer = 0;

		animator.SetTrigger(animationKey);
		List<Health> healths = new List<Health>();
		
		
		RaycastHit[] hits = Physics.SphereCastAll(origin, rad, direction, range, layers);
		
		if(hits.Length == 0)
			soundPlayer.Play();

		foreach (RaycastHit hit in hits)
		{
			HitBox hb;
			if (hit.collider.TryGetComponent(out hb))
			{
				if (healths.Contains(hb.health))
					continue;
				healths.Add(hb.health);
				playerStats.GainPoints(points);
				hb.health.TakeDmg(damage * playerStats.damageMulti, HitType.GUN);
				
				

			}
			if (hit.collider.TryGetComponent(out Surface surface))
			{
				if (hit.point == Vector3.zero)
				{
					Vector3 pos = hit.collider.ClosestPoint(origin);
					surface.data.PlayMeleeHitVfx(pos, pos - origin);

				}
				else
				{
					surface.data.PlayHitVfx(hit.point, hit.normal);
				}
			}
			else
			{
				if (hit.point == Vector3.zero)
				{
					Vector3 pos = hit.collider.ClosestPoint(origin);
					VfxSpawner.DefaultSurfaceData.PlayMeleeHitVfx(pos, pos - origin);

				}
				else
				{
					VfxSpawner.DefaultSurfaceData.PlayHitVfx(hit.point, hit.normal);
				}
			}
		}
	}

	public void Update()
	{
		timer += Time.deltaTime;
	}
	
}
