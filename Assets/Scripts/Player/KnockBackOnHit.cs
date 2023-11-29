using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="OnHit/Knockback")]
public class KnockBackOnHit : OnHitEffect
{
    [SerializeField] float magnitude;
    public override void Hit(HitBox hb, Vector3 point, Vector3 dir)
    {
        if(hb.health.TryGetComponent(out DemonFramework demon))
        {
            demon.ApplyForce(dir * magnitude);
        }
    }
}
