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

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if(health == null)
        health = GetComponentInParent<Health>();
        animator = GetComponentInParent<Animator>();
    }
    public void OnHit(float dmg, HitType type)
    {
        health.TakeDmg(dmg, type);
        animator.ResetTrigger("Hit");

        int hitNum = Random.Range(0, 7);

        animator.SetInteger("HitNum", hitNum);
        animator.SetTrigger("Hit");
    }
}
