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
    public Rigidbody rb;
    
    [SerializeField] InputActionProperty input;

    public int heldGunIndex = 0;
    public int lastGunIndex = 0;
    public Gun HeldGun { 
        get { return guns[heldGunIndex]; }
        set { SetGun(heldGunIndex, value); }
    }
    const int MaxGuns = 2;
    //[HideInInspector]
    public Gun[] guns = new Gun[MaxGuns];

    public delegate void OnDealDamageAction(float amount);
    public OnDealDamageAction OnDealDamage;

    public Movement.PlayerInput playerInput;


    [SerializeField] float frequncey =1;
    [SerializeField] float damping=1;
    [SerializeField] float reaction=1;
    public SecondOrderDynamics verticalRecoilDynamics;
    public SecondOrderDynamics horizontalRecoilDynamics;
    public bool updateKVals;
    float damageDealt;
    bool holstering = false;
    bool drawing = false;
    [Header("Animation")]
    public Animator animator;
    public string drawTrigger;
    public string holsterTigger;

    float drawTimer = 0;
    private void Start()
	{
        verticalRecoilDynamics = new SecondOrderDynamics(frequncey, damping, reaction, 0);
        horizontalRecoilDynamics = new SecondOrderDynamics(frequncey, damping, reaction, 0);
        input.action.performed += SwapGun;
        int j = 0;
        for(int i = 0; i < transform.childCount; i++)
		{
            Gun g;
			if (transform.GetChild(i).TryGetComponent(out g))
			{
                SetGun(j, g);
                j++;
			}
		}
        //OnDraw();
        OnHolster();
        animator.SetTrigger(drawTrigger);
    }
	private void Update()
	{
		if (updateKVals)
		{
            verticalRecoilDynamics.UpdateKVals(frequncey, damping, reaction);
            horizontalRecoilDynamics.UpdateKVals(frequncey, damping, reaction);
        }

        if (drawing)
        {
            drawTimer-=Time.deltaTime;
            if(drawTimer < 0)
            {
                OnDraw();
            }
        }
        
	}
	public void SetGun(int slot,Gun gun)
	{
        if (slot > MaxGuns)
            return;
        for(int i=0; i < MaxGuns; i++)
		{
            if (guns[i] == null)
			{
                slot = i;
                break;
            }
                
		}
        if(guns[slot] != null)
		{
            Destroy(guns[slot].gameObject);
		}
        guns[slot] = gun;
        gun.holster = this;
        if (gun.visualiserPool.Enabled && !gun.useOwnVisualiser)
            gun.visualiserPool.Value = bulletVisualierPool;

        SetGunIndex(slot);
    }

    public void SetGunIndex(int index)
	{
        if (guns[index] == null || heldGunIndex == index || drawing)
		{
            return;
        }
        drawing = true;
        lastGunIndex = heldGunIndex;
        heldGunIndex = index;
        animator.SetTrigger(holsterTigger);
        //      for(int i = 0; i < guns.Length; i++)
        //{
        //          if (guns[i] != null)
        //	{
        //              guns[i].gameObject.SetActive(i==index);
        //	}
        //}
        
    }
    
    
    public void OnHit(float damage,float targetMaxHealth)
	{
        
        abilityCaster.AddBlood((damage * 100 * HeldGun.bloodGainMulti * stats.bloodGainMulti)/targetMaxHealth);
        if(OnDealDamage != null)
        OnDealDamage(damage);
	}

	private void OnEnable()
	{
		input.action.Enable();
	}

	private void OnDisable()
	{
		input.action.Disable();
	}

    void SwapGun(InputAction.CallbackContext callback)
	{
        int i = heldGunIndex + 1;
        if(i >= guns.Length)
		{
            i = 0;
		}
        SetGunIndex(i);
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

    public void OnHolster()
    {
        Debug.Log("Holster");
        guns[lastGunIndex].gameObject.SetActive(false);
        guns[heldGunIndex].gameObject.SetActive(true);
        animator.ResetTrigger(holsterTigger);
        guns[heldGunIndex].gunState = Gun.GunStates.disabled;
        animator.runtimeAnimatorController = guns[heldGunIndex].controller;
        animator.SetTrigger(drawTrigger);
    }
    public void OnDraw()
    {
        Debug.Log("Draw");
        guns[heldGunIndex].gunState = Gun.GunStates.awaiting;
        drawing = false;
    }
    public bool CanShoot()
	{
        return playerInput.Running();
	}
}
