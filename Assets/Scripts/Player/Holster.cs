using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Movement;
using System;

public class Holster : MonoBehaviour
{
    public PlayerStats stats;
    public ObjectPooler bulletVisualierPool;
    public AbilityCaster abilityCaster;
    public WeaponInfo weaponInfo;
    public Rigidbody rb;
    public Transform aimTarget;
    
    [SerializeField] InputActionProperty input;

    public int heldGunIndex = 0;
    public int newGunIndex = 0;

    public int gunCount = 0;
    public Gun HeldGun { 
        get { return guns[heldGunIndex]; }
        set { SetGun(heldGunIndex, value); }
    }
    public Gun OffHandGun
    {
        get { return guns[GetOffHandIndex()]; }
        
    }

	Gun replacingGun;
	const int MaxGuns = 2;
    //[HideInInspector]
    public Gun[] guns = new Gun[MaxGuns];

    public delegate void OnDealDamageAction(float amount,HitBox hitBox);
    public OnDealDamageAction OnDealDamage;

    public Movement.PlayerInputt playerInput;


    [SerializeField] float frequncey =1;
    [SerializeField] float damping=1;
    [SerializeField] float reaction=1;
    public SecondOrderDynamics verticalRecoilDynamics;
    public SecondOrderDynamics horizontalRecoilDynamics;
    public bool updateKVals;
    float damageDealt;
    enum HolsterStates 
    {
		normal,
		drawing,
        holstering,
        replacing
    }
    [SerializeField]HolsterStates state;
    [Header("Animation")]
    public Animator animator;
    public string drawTrigger;
    public string holsterTigger;
    public bool consumeAmmo = true;
    public float drawTimer = 0;
	private void OnEnable()
	{
		input.action.Enable();
        DrawGun();
	}

	private void OnDisable()
	{
		input.action.Disable();
	}
	private void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        stats = GetComponentInParent<PlayerStats>();
        abilityCaster = GetComponentInParent<AbilityCaster>();
        playerInput = GetComponentInParent<Movement.PlayerInputt>();
    }

    void ResetRecoil()
    {
		verticalRecoilDynamics = new SecondOrderDynamics(frequncey, damping, reaction, 0);
		horizontalRecoilDynamics = new SecondOrderDynamics(frequncey, damping, reaction, 0);
	}
    private void Start()
	{
		ResetRecoil();
		input.action.performed += SwapGun;
        gunCount = 0;
        int j = 0;
        for(int i = 0; i < transform.childCount; i++)
		{
            Gun g;
			if (transform.GetChild(i).TryGetComponent(out g))
			{
                guns[i] = g;
                SetUpGun(g);
                g.gameObject.SetActive(false);
                j++;
			}
		}
		//OnDraw();


		//SetGunIndex(0);
		guns[heldGunIndex].gameObject.SetActive(true);
		animator.runtimeAnimatorController = guns[heldGunIndex].controller;
        DrawGun();

	}
	private void Update()
	{
		if (updateKVals)
		{
            verticalRecoilDynamics.UpdateKVals(frequncey, damping, reaction);
            horizontalRecoilDynamics.UpdateKVals(frequncey, damping, reaction);
        }
        drawTimer-= Time.deltaTime;

        switch (state)
        {
            case HolsterStates.normal:
                break;
            case HolsterStates.holstering:
                if(drawTimer < 0)
                {

                    guns[heldGunIndex].gameObject.SetActive(false);
                    heldGunIndex = newGunIndex;
					animator.runtimeAnimatorController = HeldGun.controller;
					guns[heldGunIndex].gameObject.SetActive(true);
                    DrawGun();
				}
                break;
            case HolsterStates.drawing:
                if(drawTimer < 0)
                {
                    state = HolsterStates.normal;
                }
                break;
            case HolsterStates.replacing:
				if (drawTimer < 0)
				{

					Destroy(HeldGun.gameObject);
                    gunCount--;
                    guns[heldGunIndex] = replacingGun;
					animator.runtimeAnimatorController = HeldGun.controller;
					guns[heldGunIndex].gameObject.SetActive(true);
					DrawGun();
				}
				break;
        }
        
	}

    void HolsterGun()
    {
        state = HolsterStates.holstering;

        animator.SetFloat(HeldGun.unEquipSpeedKey, 1 / HeldGun.holsterTime);
        animator.SetTrigger(holsterTigger);
        if (HeldGun.animator.Enabled)
        {
			HeldGun.animator.Value.SetFloat(HeldGun.unEquipSpeedKey, 1 / HeldGun.holsterTime);
			HeldGun.animator.Value.SetTrigger(holsterTigger);
		}
        drawTimer = HeldGun.holsterTime;
    }

    public void ReplaceGun(Gun gun)
    {
        replacingGun = gun;
		state = HolsterStates.replacing;

		animator.SetFloat(HeldGun.unEquipSpeedKey, 1 / HeldGun.holsterTime);
		animator.SetTrigger(holsterTigger);
		if (HeldGun.animator.Enabled)
		{
			HeldGun.animator.Value.SetFloat(HeldGun.unEquipSpeedKey, 1 / HeldGun.holsterTime);
			HeldGun.animator.Value.SetTrigger(holsterTigger);
		}
		drawTimer = HeldGun.holsterTime;
	}
    void DrawGun()
    {
		state = HolsterStates.drawing;

		animator.SetFloat(HeldGun.equipSpeedKey, 1 / HeldGun.drawTime);
		animator.SetTrigger(drawTrigger);
		if (HeldGun.animator.Enabled)
		{
			HeldGun.animator.Value.SetFloat(HeldGun.equipSpeedKey, 1 / HeldGun.drawTime);
			HeldGun.animator.Value.SetTrigger(drawTrigger);
		}
		drawTimer = HeldGun.drawTime;
	}
    

	public void SetGun(int slot,Gun gun)
	{
        if (slot > MaxGuns)
            return;
		gun.gameObject.SetActive(false);
		for (int i=0; i < MaxGuns; i++)
		{
            if (guns[i] == null)
			{
                slot = i;
				SetUpGun(gun);
                guns[slot] = gun;
				SetGunIndex(slot);
				
				return;
            }
                
		}
        
  
        SetUpGun(gun);
        ReplaceGun(gun);
        
	}

    public void SetUpGun(Gun gun)
    {
		gun.holster = this;
		if (gun.visualiserPool.Enabled && !gun.useOwnVisualiser)
			gun.visualiserPool.Value = bulletVisualierPool;
        gunCount++;
	}

    
    

    public void SetGunIndex(int index)
	{
        if (guns[index] == null || heldGunIndex == index || state != HolsterStates.normal)
		{
            return;
        }
        newGunIndex = index;
        HolsterGun();
    }
    
    
    public void OnHit(float damage,HitBox hitBox)
	{
        
        abilityCaster.AddBlood(Mathf.Clamp01(damage/hitBox.health.maxHealth) * HeldGun.bloodGainMulti * stats.bloodGainMulti);
        if(OnDealDamage != null)
        OnDealDamage(damage,hitBox);
	}

	int GetOffHandIndex()
    {
		int i = heldGunIndex + 1;
		if (i >= guns.Length)
		{
			i = 0;
		}
        return i;
	}

    void SwapGun(InputAction.CallbackContext callback)
	{
        int i = heldGunIndex + 1;
        if(i >= guns.Length)
		{
            i = 0;
		}
        SetGunIndex(i);
        
        //weaponInfo.WeaponSwapTween();
    }

    public bool HasGun(Gun g)
    {
        Gun emptyGun;
        return HasGun(g, out emptyGun);
        
    }
    public bool HasGun(Gun g,out Gun returnedGun)
    {
        foreach (Gun gun in guns)
        {
            if (gun == null)
                continue;
            if (g.guid == gun.guid)
            {
                returnedGun = gun;
                return true;
            }
                
        }
        returnedGun = null;
        return false;
    }

   
    public bool CanShoot()
	{
        return playerInput.Running();
	}
}
