using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAnimationOverrides : MonoBehaviour
{
    [Header("Animation Overwrite")]
    [SerializeField] private List<AnimatorControllerOverrides> animatorControllers = new List<AnimatorControllerOverrides>();
    private AnimatorControllerOverrides currentControllerOverride;

    private AnimatorOverrideController RandomController(Animator animator)
    {
        int num = Random.Range(0, animatorControllers.Count);

        AnimatorControllerOverrides contoller = animatorControllers[num];

        while(contoller == currentControllerOverride)
        {
            num = Random.Range(0, animatorControllers.Count);
            contoller = animatorControllers[num];
        }

        num = Random.Range(0, contoller.Overrides.Count);

        RuntimeAnimatorController temp = animator.runtimeAnimatorController;
        RuntimeAnimatorController temp1 = contoller.Overrides[num];

        while (temp == temp1)
        {
            num = Random.Range(0, animatorControllers.Count);
            temp1 = contoller.Overrides[num];
        }

        currentControllerOverride = contoller;

        return contoller.Overrides[num];
    }

    public AnimatorOverrideController SetOverrideController(Animator animator)
    {
         return RandomController(animator);
    }
}

[System.Serializable]
public class AnimatorControllerOverrides
{
    public RuntimeAnimatorController controller;
    public List<AnimatorOverrideController> Overrides = new List<AnimatorOverrideController>();
}