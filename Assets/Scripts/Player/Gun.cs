using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Gun : MonoBehaviour
{

    public enum GunStates
	{
        awaiting,
        firing,
        reloading,
        disabled
	}
    GunStates gunState;
    
    public enum FireTypes
    {
        single,
        burst,
        auto
    }
    [Header("Gun Settings")]
    public FireTypes fireSelect;
    public float damage = 10;
    public float bulletRange = float.PositiveInfinity, bulletSpreadDegrees = 0;
    [Min(1)]
    public int shotsPerFiring = 1;
    public int ammoLeft, maxAmmo = 10;
    public float fireCoolDown = 1, reloadDuration = 1;
    float fireTimer = 0, reloadTimer;

    [Min(1)]
    public int burstRounds;
    int ammoBurstsDone = 0;
    public float burstCooldown = 0.1f;
    float burstTimer =0;

    
    [Header("Assign These")]
    public Transform origin;
    public SoundPlayer shootSound, reloadSound, emptySound;
    public Optional<ParticleSystem> gunfire;
    public LayerMask hitMask = int.MaxValue;
    
    
    
    
    public InputActionProperty shootAction;
    public InputActionProperty reloadAction;
    
    public Optional<ObjectPooler> visualiserPool;
    public float bulletVisualiserSpeed;

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

		switch (gunState)
		{
            case GunStates.awaiting:
                if (fireTimer <= 0)
                {
                    switch (fireSelect)
                    {
                        case FireTypes.single:
                            if (shootAction.action.WasPressedThisFrame())
                                Shoot();
                            break;
                        case FireTypes.burst:
                            if (shootAction.action.WasPressedThisFrame())
                            {
                                Shoot();
                                ammoBurstsDone++;
                                gunState = GunStates.firing;
                            }

                            
                            break;
                        case FireTypes.auto:
                            if (shootAction.action.IsPressed())
                                Shoot();
                            break;
                    }
                    
                }
                break;
            case GunStates.firing:
                if(fireSelect == FireTypes.burst)
				{
                    burstTimer -= Time.deltaTime;
                    if(ammoBurstsDone >= burstRounds)
					{
                        gunState = GunStates.awaiting;
                        ammoBurstsDone = 0;
                        burstTimer = burstCooldown;
                        break;
                    }
                    if(burstTimer <=0 )
					{
                        burstTimer = burstCooldown;
                        Shoot();
                        ammoBurstsDone++;
					}
				}
                break;
            case GunStates.reloading:
                reloadTimer += Time.deltaTime;
                if (reloadTimer >= reloadDuration)
                {
                    Reload();
                }
                break;
            case GunStates.disabled:
                break;
		}


		

        
    }

    public void Shoot()
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
                            vfx.Play(hit.point, Vector3.Lerp(-Camera.main.transform.forward, hit.normal,.5f));
                        }
                        else
                        {
                            VfxSpawner.SpawnVfx(0, hit.point, Vector3.Lerp(-Camera.main.transform.forward, hit.normal, .5f));
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
            if(ammoLeft == 0)
			{
                emptySound.Play();
                ammoLeft--;
            }
            

        

    }

    
    

    public void StartReload(InputAction.CallbackContext context)
    {
        
        if (gunState != GunStates.awaiting || ammoLeft == maxAmmo)
            return;
        gunState = GunStates.reloading;
        reloadTimer = 0;
        reloadSound.Play();
    }
    void Reload()
    {
        ammoLeft = maxAmmo;
        gunState = GunStates.awaiting;

    }
}
