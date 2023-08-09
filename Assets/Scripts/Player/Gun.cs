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
    public string gunName;
    public FireTypes fireSelect;
    public float damage = 10;
    public float bulletRange = float.PositiveInfinity;
    [Min(1)]
    public int shotsPerFiring = 1;
    public int ammoLeft, maxAmmo = 10;
    public float roundsPerMin = 1, reloadDuration = 1;
    float fireTimer = 0, reloadTimer;
    public float bloodGainMulti = 1;

    [Header("SpreadSettings")]
    public float bulletSpreadDegrees = 0;
    [Tooltip("Recoil Increments By 1 for each shot and decays with recoilResetSpeed")]
    public AnimationCurve recoilSpreadCurve;
    public AnimationCurve velocitySpredCurve;
    public float maxRecoilVal = 30;
    public float recoilResetSpeed = 1;
    public float recoilResetDelay;
    public float recoil;
    float timeSinceLastShot;
    [Header("Burst Settings")]
    [Min(1)]
    public int burstRounds;
    int ammoBurstsDone = 0;
    public float burstRpm = 0.1f;
    float burstTimer =0;

    [Header("Penetration Settings")]
    public int maxPenetrations = 3;
    
    public float damageLossDivisor = 2;

    [Header("Stash Settings")]
    public int stash;
    public int maxStash;

    
    [Header("Assign These")]
    public Transform origin;
    public SoundPlayer shootSound, reloadSound, emptySound;
    public Optional<ParticleSystem> gunfire;
    public LayerMask hitMask = int.MaxValue;
    
    
    
    
    public InputActionProperty shootAction;
    public InputActionProperty reloadAction;

    public Holster holster;

    public bool useOwnVisualiser;
    public Optional<ObjectPooler> visualiserPool;
    public float bulletVisualiserSpeed;

    [Header("Animation")]
    public Optional<Animator> animator;
    public string shootKey = "shoot";
    public string reloadKey = "reload";
    public string meleeKey = "melee";
    public string sprintKey = "sprinting";

    // Start is called before the first frame update
    void Start()
    {
        ammoLeft = maxAmmo;
        reloadAction.action.performed += StartReload;
        //GetComponentInParent<Holster>().SetGun(transform.GetSiblingIndex(),this);
    }

	private void OnDestroy()
	{
        reloadAction.action.performed -= StartReload;
        if(visualiserPool.Enabled && visualiserPool.Value != null)
            visualiserPool.Value.DespawnAll();
    }

	private void OnEnable()
	{
		shootAction.action.Enable();
        reloadAction.action.Enable();
	}
	private void OnDisable()
	{
        //shootAction.action.Disable();
        //reloadAction.action.Disable();
    }
	private void Update()
    {
        fireTimer -= Time.deltaTime;
        
        timeSinceLastShot+= Time.deltaTime;
        if (timeSinceLastShot > recoilResetDelay)
            recoil = Mathf.Clamp(recoil - Time.deltaTime * recoilResetSpeed, 0, maxRecoilVal);
		switch (gunState)
		{
            case GunStates.awaiting:
                if (fireTimer <= 0)
                {
                    switch (fireSelect)
                    {
                        case FireTypes.single:
                            if (shootAction.action.WasPressedThisFrame())
                            {
                                if (ammoLeft <= 0)
                                {
                                    if (stash <= 0)
                                    {
                                        emptySound.Play();
                                    }
									else
									{
                                        emptySound.Play();
                                        StartReload();
									}
                                }
								else
								{
                                    Shoot();
                                }
                                    
                            }
                            break;
                        case FireTypes.burst:
                            if (shootAction.action.WasPressedThisFrame())
                            {
                                if (ammoLeft <= 0)
                                {
                                    if (stash <= 0)
                                    {
                                        emptySound.Play();
                                    }
                                    else
                                    {
                                        emptySound.Play();
                                        StartReload();
                                    }
                                }
                                else
                                {
                                    Shoot();
                                    ammoBurstsDone++;
                                    gunState = GunStates.firing;
                                }


                                
                            }
                            break;
                        case FireTypes.auto:
                            if (shootAction.action.IsPressed())
                            {
                                if (ammoLeft <= 0)
                                {
                                    if (stash <= 0)
                                    {
                                        if (shootAction.action.WasPressedThisFrame())
                                            emptySound.Play();
                                    }
                                    else
                                    {
                                        emptySound.Play();
                                        StartReload();
                                    }
                                }
                                else
                                {
                                    Shoot();
                                }

                            }
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
                        burstTimer = 1/(burstRpm/60);
                        break;
                    }
                    if(burstTimer <=0 )
					{
                        burstTimer = 1/(burstRpm/60);
                        if(ammoLeft <= 0)
						{
                            ammoBurstsDone = burstRounds;
						}
						else
						{
                            Shoot();
                            ammoBurstsDone++;
                        }
                        
					}
				}
                break;
            case GunStates.reloading:
                reloadTimer += Time.deltaTime;
                if (reloadTimer >= reloadDuration * holster.stats.reloadTimeMulti)
                {
                    Reload();
                }
                break;
            case GunStates.disabled:
                break;
		}

    }

    protected virtual void Shoot()
    {


        for (int i = 0; i < shotsPerFiring; i++)
        {
            if (animator.Enabled)
                animator.Value.SetTrigger(shootKey);
            Vector3 randVal = Random.insideUnitSphere * GetSpread();
            Vector3 dir = Quaternion.Euler(randVal) * Camera.main.transform.forward;
            Debug.DrawRay(Camera.main.transform.position, dir * 10, Color.green);

            RaycastHit[] hitArray = Physics.RaycastAll(Camera.main.transform.position, dir, bulletRange, hitMask);
            if (hitArray.Length > 0)
            {
                float currentDamage = damage;
                int penIndex = 0;

                //Reorder raycast hits in order of hit
                List<RaycastHit> hits = new List<RaycastHit>();
                for (int j = 0; j < hitArray.Length; j++)
                { 
                    hits.Add(hitArray[j]);
                }
                List<Health> healths = new List<Health>();
                hits.Sort((a, b) => (a.distance.CompareTo(b.distance)));
                int completePens = 0;


                for (int j = 0; j < hits.Count; j++)
                {
                    if (completePens > maxPenetrations)
                        break;
                    RaycastHit hit = hits[j];
                    //Debug.Log(hit.transform);
                    
                    penIndex = j;

                    bool playFx = true;

                    bool penetrable = false;
                    HitBox hitBox;
                    if (hit.collider.TryGetComponent(out hitBox))
                    {
                        
                        if (healths.Contains(hitBox.health))
                        {
                            playFx = false;
                        }
						else
						{
                            healths.Add(hitBox.health);
                            hitBox.OnHit(currentDamage * holster.stats.damageMulti);
                            holster.OnHit((damage * holster.stats.damageMulti * hitBox.multi)/hitBox.health.maxHealth);
                        }
                        

                    }
                    HitSettings hitSettings;
                    if (hit.collider.TryGetComponent(out hitSettings))
                    {
                        if (hitSettings.Penetrable)
                            penetrable = true;
                        if(playFx)
                        hitSettings.PlayVfx(hit.point, Vector3.Lerp(-dir, hit.normal, .5f));
                    }
                    else
                    {
                        if(playFx)
                        VfxSpawner.SpawnVfx(0, hit.point, Vector3.Lerp(-dir, hit.normal, .5f));
                    }



                    currentDamage /= damageLossDivisor;
                    if (!penetrable)
                    {
                        break;
                    }
                    completePens++;
                }
                if (visualiserPool.Enabled)
                {
                    visualiserPool.Value.Spawn().GetComponent<BulletVisualiser>().Shoot(origin, hits[penIndex].point, Vector3.Distance(origin.position, hits[penIndex].point) / bulletVisualiserSpeed,dir);
                }
            }

            else
            {
                if (visualiserPool.Enabled)
                    visualiserPool.Value.Spawn().GetComponent<BulletVisualiser>().Shoot(origin, Camera.main.transform.forward * 1000, 1000 / bulletVisualiserSpeed,dir);
            }
            
        }
                
        ammoLeft--;
        fireTimer = 1/(roundsPerMin/60);
        recoil++;
        if(recoil > maxRecoilVal)
            recoil = maxRecoilVal;
        timeSinceLastShot = 0;
        shootSound.Play();
        if (gunfire.Enabled)
        {
            gunfire.Value.Play();
        }
            
            
    }

    
    
    float GetSpread()
    {
        float spread = bulletSpreadDegrees;
        spread += recoilSpreadCurve.Evaluate(recoil);
        spread += velocitySpredCurve.Evaluate(holster.rb.velocity.magnitude);
        return spread;
    }
    public void StartReload(InputAction.CallbackContext context)
    {
        StartReload();
        if (animator.Enabled)
            animator.Value.SetTrigger(reloadKey);
    }

    public void StartReload()
	{
        if (gunState != GunStates.awaiting || ammoLeft == maxAmmo || stash <= 0)
            return;
        gunState = GunStates.reloading;
        reloadTimer = 0;
        reloadSound.Play();
    }
    void Reload()
    {
        int ammoDrawn = maxAmmo - ammoLeft;
        if (stash >= ammoDrawn)
		{
            ammoLeft = maxAmmo;
            stash -= ammoDrawn;
        }
		else
		{
            ammoLeft = stash;
            stash -= ammoLeft;
		}
        

        gunState = GunStates.awaiting;

    }

    public void AddToStash(int amount)
	{
        stash += amount;
        if(stash >= maxStash)
		{
            stash = maxStash;
		}
	}

    public void AddToStashPercent(float percent)
	{
        percent = Mathf.Clamp01(percent);
        int ammoToAdd = Mathf.RoundToInt(percent * (float)maxStash);
        AddToStash(ammoToAdd);
	}

    public void RefillStash()
	{
        AddToStash(1);
	}
}
