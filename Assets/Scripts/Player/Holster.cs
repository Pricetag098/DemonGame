using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holster : MonoBehaviour
{

    public PlayerStats stats;
    public ObjectPooler bulletVisualierPool;
    Gun gun;
    public Gun HeldGun { get { return gun; } set
        {
            if(gun != null)
            Destroy(gun.gameObject);
            gun = value;
            gun.holster = this;
            if(gun.visualiserPool.Enabled)
            gun.visualiserPool.Value = bulletVisualierPool;
        }
    }
    
}
