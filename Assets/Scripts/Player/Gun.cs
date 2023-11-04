using System;
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
    public GunStates gunState;
    
    public enum FireTypes
    {
        single,
        burst,
        auto
    }
    [Header("Gun Settings")]
    public string gunName;
    public string guid;
    public FireTypes fireSelect;
    public float bulletRange = float.PositiveInfinity;
    [Min(1)]
    public int shotsPerFiring = 1;
    public int ammoLeft, maxAmmo = 10;
    public float roundsPerMin = 1, reloadDuration = 1;
    float fireTimer = 0, reloadTimer;
    public float bloodGainMulti = 1;
    public float drawTime = 1;
    public float holsterTime = 1;
    [SerializeField] List<OnHitEffect> onHitEffectList = new List<OnHitEffect>();

    [Header("DamageSetting")]
    public float headDamage = 10;
    public float bodyDamage = 10;
    public float limbDamage = 10;
    public float critDamage = 10;

    [Header("PointsSetting")]
    public int headPoints = 10;
    public int bodyPoints = 10;
    public int limbPoints = 10;
    public int critPoints = 10;


    [Header("SpreadSettings")]
    public float bulletSpreadDegrees = 0;
    [Tooltip("Recoil Increments By 1 for each shot and decays with recoilResetSpeed")]
    public AnimationCurve verticalRecoilSpreadCurve;
    public AnimationCurve horizontalRecoilSpreadCurve;
    public AnimationCurve bloomRecoilSpreadCurve;
    public AnimationCurve velocitySpredCurve;
    //public float maxRecoilVal = 30;
    //public float recoilResetSpeed = 1;
    //public float recoilResetDelay;
    public float recoil;
    float timeSinceLastShot;
    public bool smoothRecoil = true;
    public float recoilEffectDuration = .1f;
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
    public RuntimeAnimatorController controller;
    public string shootKey = "shoot";
    public string reloadKey = "reload";
    public string shootspeedKey = "FireRate";
    public string reloadSpeedKey = "ReloadRate";
    public string equipSpeedKey = "EquipRate";
    public string unEquipSpeedKey = "UnEquipRate";
	public string fireIndexKey = "FireIndex";
	public int fireAnimations = 1;
    //public string sprintKey = "sprinting";

    [Header("Upgrading")]
    public Optional<GunUpgradePath> path;
    public int tier = 0;

    public Vector3 test;
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
    [ContextMenu("Gen Guid")]
    void GenGuid()
	{
        guid = Guid.NewGuid().ToString();
	}
	private void OnEnable()
	{
		shootAction.action.Enable();
        reloadAction.action.Enable();
        
        if (animator.Enabled)
        {
            animator.Value.SetFloat(equipSpeedKey, 1 / drawTime);
        }
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
        
        if(!smoothRecoil)
            holster.playerInput.SetRecoil(Vector3.zero);

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
				if (reloadTimer > reloadDuration * holster.stats.reloadTimeMulti)
                {
                    Debug.Log(reloadTimer);
                    Reload();
                }
				
				break;
            case GunStates.disabled:
                break;
		}

        if (timeSinceLastShot > 1 / (roundsPerMin / 60))
        {
            recoil = Mathf.Clamp(recoil - Time.deltaTime * maxAmmo, 0, maxAmmo);
            
        }

        if(timeSinceLastShot > recoilEffectDuration)
		{
            if(Time.timeScale != 0)
            {
				holster.playerInput.SetRecoil(
				new Vector3(
					holster.verticalRecoilDynamics.Update(Time.deltaTime, 0),
					holster.horizontalRecoilDynamics.Update(Time.deltaTime, 0)
					, 0));
			}
            
        }
        else
        {
            if (Time.timeScale != 0)
            {
                holster.playerInput.SetRecoil(
            new Vector3(
                    holster.verticalRecoilDynamics.Update(Time.deltaTime, -verticalRecoilSpreadCurve.Evaluate(recoil)),
                    holster.horizontalRecoilDynamics.Update(Time.deltaTime, horizontalRecoilSpreadCurve.Evaluate(recoil))
                    , 0));
            }
		}
    }

    protected virtual void Shoot()
    {

       
        for (int i = 0; i < shotsPerFiring; i++)
        {
            
            Vector3 randVal = GetSpread(UnityEngine.Random.insideUnitSphere);
            
            
            Vector3 dir = holster.aimTarget.rotation * (Quaternion.Euler(randVal) * Vector3.forward);
            Debug.DrawRay(holster.aimTarget.position, dir * 10, Color.green);

            RaycastHit[] hitArray = Physics.RaycastAll(holster.aimTarget.position, dir, bulletRange, hitMask);
            if (hitArray.Length > 0)
            {
                float damageMulti = 1;
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
                        //check to avoid double hits on penetration
                        if (healths.Contains(hitBox.health))
                        {
                            playFx = false;
                        }
						else
						{
                            //Do all on hit stuff here

                            float damage = GetDamage(hitBox.bodyPart) * damageMulti * holster.stats.damageMulti;

                            healths.Add(hitBox.health);
                            hitBox.OnHit(damage);
                            holster.stats.GainPoints(GetPoints(hitBox.bodyPart));
                            holster.OnHit(damage, hitBox.health.maxHealth);
                        }
                        

                    }

                    SurfaceData surfaceData;
                    Surface surface;
                    if (hit.collider.TryGetComponent(out surface))
                    {
                        if (surface.Penetrable)
                            penetrable = true;

                        surfaceData = surface.data;
                    }
					else
					{
                        surfaceData = VfxSpawner.DefaultSurfaceData;
					}
                    surfaceData.PlayHitVfx(hit.point, Vector3.Lerp(-dir, hit.normal, .5f));

                    damageMulti /= damageLossDivisor;
                    if (!penetrable)
                    {
                        break;
                    }
                    completePens++;
                }
                if (visualiserPool.Enabled)
                {
                    visualiserPool.Value.Spawn().GetComponent<BulletVisualiser>().Shoot(origin.position, hits[penIndex].point, Vector3.Distance(origin.position, hits[penIndex].point) / bulletVisualiserSpeed,dir);
                }
            }

            else
            {
                if (visualiserPool.Enabled)
                    visualiserPool.Value.Spawn().GetComponent<BulletVisualiser>().Shoot(origin.position, Camera.main.transform.forward * 1000, 1000 / bulletVisualiserSpeed,dir);
            }
            
        }
        if(holster.consumeAmmo)
        ammoLeft--;
        fireTimer = 1/(roundsPerMin/60);
        recoil++;
        if(recoil > maxAmmo)
            recoil = maxAmmo;
        timeSinceLastShot = 0;
        shootSound.Play();

        holster.animator.SetTrigger(shootKey);
        holster.animator.SetFloat(shootspeedKey,1/ fireTimer);
        if (animator.Enabled)
        {
            animator.Value.SetInteger(fireIndexKey,ammoLeft % fireAnimations);
            animator.Value.SetTrigger(shootKey);
            animator.Value.SetFloat(shootspeedKey,1/ fireTimer);
        }
            
        if (gunfire.Enabled)
        {
            gunfire.Value.Play();
        }
        if(!smoothRecoil)
            holster.playerInput.SetRecoil(new Vector3(-verticalRecoilSpreadCurve.Evaluate(recoil), horizontalRecoilSpreadCurve.Evaluate(recoil), 0));

    }

    public void StartEquip()
    {

    }
    
    

    #region Reloading
    public void StartReload(InputAction.CallbackContext context)
    {
        StartReload();
        
    }

    public void StartReload()
	{
        if (gunState != GunStates.awaiting || ammoLeft == maxAmmo || stash <= 0)
            return;
        if (animator.Enabled)
        {
            animator.Value.SetTrigger(reloadKey);
            animator.Value.SetFloat(reloadSpeedKey, 1/reloadDuration);
        }
        holster.animator.SetFloat(reloadSpeedKey,1/reloadDuration);
        Debug.Log(reloadDuration);
        holster.animator.SetTrigger(reloadKey);
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
        AddToStashPercent(1);
	}
    #endregion
    #region GunData Evaluation
    public Vector3 GetSpread(Vector3 rand)
    {
        
        rand.z = 0;


        Vector3 spread = Vector3.zero;
        spread += rand * velocitySpredCurve.Evaluate(holster.rb.velocity.magnitude);
        spread += rand * bulletSpreadDegrees;
        spread += rand * bloomRecoilSpreadCurve.Evaluate(recoil);
        //spread.y = 0;
        return spread;
    }
    public float GetDamage(HitBox.BodyPart bodyPart)
	{
		switch (bodyPart)
		{
            case HitBox.BodyPart.Head:
                return headDamage;
            case HitBox.BodyPart.Limb:
                return limbDamage;
            case HitBox.BodyPart.Crit:
                return critDamage;
            default:
                return bodyDamage;
		}
	}
    public int GetPoints(HitBox.BodyPart bodyPart)
    {
        switch (bodyPart)
        {
            case HitBox.BodyPart.Head:
                return headPoints;
            case HitBox.BodyPart.Limb:
                return limbPoints ;
            case HitBox.BodyPart.Crit:
                return critPoints;
            default:
                return bodyPoints;
        }
    }
    #endregion
}
