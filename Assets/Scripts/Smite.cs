using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Smite : MonoBehaviour
{
    public float charge;
    public float maxCharge;
    [Range(0f, 1f)]
    public float minCharge;
    [SerializeField] float rayRange = 200;
    [SerializeField] AnimationCurve damageCurve;
    [SerializeField] AnimationCurve radiusCurve;
    [SerializeField] VfxSpawnRequest vfx;
    [SerializeField] InputActionProperty input;
    [SerializeField] LayerMask rayLayers;
    [SerializeField] LayerMask damageLayers;
	// Start is called before the first frame update

	private void Start()
	{
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
	// Update is called once per frame
	void Update()
    {
        charge += Time.deltaTime;
    }

    public void Cast(InputAction.CallbackContext context)
    {
        float chargePercent = Mathf.Clamp01(charge / maxCharge);
        if (chargePercent < minCharge) return;

        
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,out RaycastHit hit, rayRange, rayLayers))
        {
			float damage = damageCurve.Evaluate(chargePercent);
			float radius = radiusCurve.Evaluate(chargePercent);
			Collider[] cols = Physics.OverlapSphere(hit.point, radius, damageLayers);
			List<Health> healths = new List<Health>();
			for (int i = 0; i < cols.Length; i++)
			{
				Collider col = cols[i];
				HitBox hb;
				if (col.TryGetComponent(out hb))
				{
					if (!healths.Contains(hb.health))
					{
						healths.Add(hb.health);
						hb.health.TakeDmg(damage);
						
					}
				}
			}
			charge = 0;
            vfx.Play(hit.point,Camera.main.transform.position - hit.point,Vector3.one * radius);
		}

    }
}
