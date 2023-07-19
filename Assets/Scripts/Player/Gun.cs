using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Gun : MonoBehaviour
{
    [SerializeField] bool auto;
    [SerializeField] float damage = 10;
    
    [SerializeField] float bulletRange = float.PositiveInfinity, bulletSpreadDegrees = 0;
    [SerializeField] int shotsPerFiring = 1;
    [SerializeField] int ammoLeft, maxAmmo = 10;
    [SerializeField] float fireCoolDown = 1, reloadDuration = 1;
    float fireTimer = 0, reloadTimer;
    bool reloading = false;
    [SerializeField] Transform origin;


    
    [SerializeField] SoundPlayer shootSound, reloadSound, emptySound;
    [SerializeField] Optional<ParticleSystem> gunfire;
    [SerializeField] LayerMask hitMask = int.MaxValue;

    


    [SerializeField] InputActionProperty shootAction;
    [SerializeField] InputActionProperty reloadAction;

    [SerializeField] Optional<ObjectPooler> visualiserPool;
    [SerializeField] float bulletVisualiserSpeed;

    // Start is called before the first frame update
    void Start()
    {
        ammoLeft = maxAmmo;
        reloadAction.action.performed += StartReload;
    }

	private void OnDestroy()
	{
        reloadAction.action.performed -= StartReload;
    }

	private void OnEnable()
	{
		shootAction.action.Enable();
        reloadAction.action.Enable();
	}
	private void OnDisable()
	{
        shootAction.action.Disable();
        reloadAction.action.Disable();
    }
	private void Update()
    {
        fireTimer -= Time.deltaTime;
        if (reloading)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= reloadDuration)
            {
                Reload();
            }
        }
        if ((shootAction.action.WasPressedThisFrame() && !auto) || (shootAction.action.IsPressed() && auto))
            Shoot();
    }

    public void Shoot()
    {
        if (fireTimer <= 0 && !reloading)
        {
            if (ammoLeft > 0)
            {
                for (int i = 0; i < shotsPerFiring; i++)
                {
                    Vector3 randVal = Random.insideUnitSphere * bulletSpreadDegrees;
                    Vector3 dir = Quaternion.Euler(randVal) * Camera.main.transform.forward;
                    Debug.DrawRay(Camera.main.transform.position, dir * 10, Color.green);

                    RaycastHit hit;
                    if (Physics.Raycast(Camera.main.transform.position, dir, out hit, bulletRange, hitMask))
                    {
                        HitBox hitBox;
                        if (hit.collider.TryGetComponent(out hitBox))
                        {
                            hitBox.OnHit(damage);

                        }
                        HitVfx vfx;
                        if (hit.collider.TryGetComponent(out vfx))
                        {
                            vfx.Play(hit.point, hit.normal);
                        }
                        else
                        {
                            VfxSpawner.SpawnVfx(0, hit.point, hit.normal);
                        }
                        if(visualiserPool.Enabled)
                        visualiserPool.Value.Spawn().GetComponent<BulletVisualiser>().Shoot(origin, hit.point,Vector3.Distance(origin.position, hit.point) / bulletVisualiserSpeed);
                    }
					else
					{
                        if (visualiserPool.Enabled)
                            visualiserPool.Value.Spawn().GetComponent<BulletVisualiser>().Shoot(origin, Camera.main.transform.forward * 1000, 1000 / bulletVisualiserSpeed);
                    }

                }
                
                ammoLeft--;
                fireTimer = fireCoolDown;
                
                shootSound.Play();
                if (gunfire.Enabled)
                {
                    gunfire.Value.Play();
                }
            }
            //play no ammo sound / fx
            emptySound.Play();
        }

    }

    
    

    public void StartReload(InputAction.CallbackContext context)
    {
        
        if (reloading || ammoLeft == maxAmmo)
            return;
        reloading = true;
        reloadTimer = 0;
        reloadSound.Play();
    }
    void Reload()
    {
        ammoLeft = maxAmmo;
        reloading = false;
       
    }
}
