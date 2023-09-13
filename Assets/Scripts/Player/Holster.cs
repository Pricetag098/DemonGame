using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Movement;
public class Holster : MonoBehaviour
{
    public PlayerStats stats;
    public ObjectPooler bulletVisualierPool;
    public AbilityCaster abilityCaster;
    public Rigidbody rb;
    
    [SerializeField] InputActionProperty input;

    public int heldGunIndex = 0;
    public Gun HeldGun { 
        get { return guns[heldGunIndex]; }
        set { SetGun(heldGunIndex, value); }
    }
    const int MaxGuns = 2;
    Gun[] guns = new Gun[MaxGuns];

    public delegate void OnDealDamageAction(float amount);
    public OnDealDamageAction OnDealDamage;

    public Movement.PlayerInput playerInput;

    float damageDealt;
	private void Start()
	{
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
        if (guns[index] == null)
		{
            return;
        }
            
        heldGunIndex = index;
        for(int i = 0; i < guns.Length; i++)
		{
            if (guns[i] != null)
			{
                guns[i].gameObject.SetActive(i==index);
			}
		}
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

    public bool CanShoot()
	{
        return playerInput.Running();
	}
}
