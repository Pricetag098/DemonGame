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
    // Start is called before the first frame update
    void Awake()
    {
        if(health == null)
        health = GetComponentInParent<Health>();
        animator.Value = GetComponentInParent<Animator>();
        animator.Enabled = animator.Value != null;
    }
    public void OnHit(float dmg, HitType type)
    {
        health.TakeDmg(dmg, type);
        

        int hitNum = Random.Range(0, 7);
        if (animator.Enabled)
        {
            animator.Value.ResetTrigger("Hit");
            animator.Value.SetInteger("HitNum", hitNum);
            animator.Value.SetTrigger("Hit");
        }
        
    }
}
