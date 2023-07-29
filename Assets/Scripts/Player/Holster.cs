using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Holster : MonoBehaviour
{
    public PlayerStats stats;
    public ObjectPooler bulletVisualierPool;
    public AbilityCaster abilityCaster;

    [SerializeField] InputActionProperty input;

    public int heldGunIndex = 0;
    public Gun HeldGun { 
        get { return guns[heldGunIndex]; }
        set { SetGun(heldGunIndex, value); }
    }
    const int MaxGuns = 2;
    Gun[] guns = new Gun[MaxGuns];

	private void Start()
	{
        input.action.performed += SwapGun;
	}

	public void SetGun(int slot,Gun gun)
	{
        if (slot > MaxGuns)
            return;
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
        heldGunIndex = index;
        for(int i = 0; i < guns.Length; i++)
		{
            if (guns[i] != null)
			{
                guns[i].gameObject.SetActive(i==index);
			}
		}
	}
    
    public void OnHit(float damage)
	{
        abilityCaster.AddBlood(damage * HeldGun.bloodGainMulti * stats.bloodGainMulti);
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
        heldGunIndex++;
        if(heldGunIndex >= guns.Length)
		{
            heldGunIndex = 0;
		}
        SetGunIndex(heldGunIndex);
	}
}
