using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationBehaviour : StateMachineBehaviour
{
    private DemonFramework m_Base;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        if(m_Base == null) m_Base = animator.gameObject.GetComponent<DemonFramework>();
        m_Base.SetAttackOverride();
    }
}
