using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnHitEffect : ScriptableObject
{
    public abstract void Hit(HitBox hb, Vector3 point, Vector3 dir);
}
