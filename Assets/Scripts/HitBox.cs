using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public float multi = 1;

    
    public Health health;
    // Start is called before the first frame update
    void Start()
    {
        if(health == null)
        health = GetComponentInParent<Health>();
    }
    public void OnHit(float dmg)
    {
        health.TakeDmg(dmg * multi);
    }
}
