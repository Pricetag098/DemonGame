using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enum BodyPart
	{
        Head,
        Body,
        Limb,
        Crit
	}
    public BodyPart bodyPart;
    
    public Health health;

    private Optional<Animator> animator;
    [HideInInspector]public Optional<Rigidbody> rigidBody;
    // Start is called before the first frame update
    void Awake()
    {
        if(health == null)
        health = GetComponentInParent<Health>();
        animator.Value = GetComponentInParent<Animator>();
        animator.Enabled = animator.Value != null;
        rigidBody.Enabled = TryGetComponent(out Rigidbody r);
        rigidBody.Value = r;
    }
    public bool OnHit(float dmg, HitType type)
    {
        
        

        int hitNum = Random.Range(0, 7);
        if (animator.Enabled)
        {
            animator.Value.ResetTrigger("Hit");
            animator.Value.SetInteger("HitNum", hitNum);
            animator.Value.SetTrigger("Hit");
        }
        return health.TakeDmg(dmg, type);
    }
}
