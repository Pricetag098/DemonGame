using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationOverrideType
{
    Null,
    Attack,
    Movement
}

public class DemonAnimationOverrides : MonoBehaviour
{
    [Header("Animation Overwrite")]
    [SerializeField] private List<AnimatorOverrideController> _attackOverrides = new List<AnimatorOverrideController>();
    [SerializeField] private List<AnimatorOverrideController> _movementOverrides = new List<AnimatorOverrideController>();

    private AnimatorOverrideController RandomController(Animator animator, AnimationOverrideType type)
    {
        List<AnimatorOverrideController> list = new List<AnimatorOverrideController>();

        switch (type)
        {
            case AnimationOverrideType.Attack:
                list = _attackOverrides;
                break;
            case AnimationOverrideType.Movement:
                list = _movementOverrides;
                break;
            case AnimationOverrideType.Null:

                break; 
        }

        int num = Random.Range(0, list.Count);

        RuntimeAnimatorController temp = animator.runtimeAnimatorController;
        RuntimeAnimatorController temp1 = list[num];

        while (temp == temp1)
        {
            num = Random.Range(0, list.Count);
            temp1 = list[num];
        }

        return list[num];
    }

    public AnimatorOverrideController SetOverrideController(Animator animator, AnimationOverrideType type)
    {
         return RandomController(animator, type);
    }
}
