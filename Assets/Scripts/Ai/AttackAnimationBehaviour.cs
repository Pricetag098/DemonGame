using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationBehaviour : StateMachineBehaviour
{
    private DemonBase m_Base;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        if(m_Base is null) m_Base = animator.gameObject.GetComponent<DemonBase>();
        m_Base.SetAttackOverride();
    }
}
