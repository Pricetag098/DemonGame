using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAnimationOverrides : MonoBehaviour
{
    [Header("Animation Overwrite")]
    [SerializeField] private List<AnimatorControllerOverrides> animatorControllers = new List<AnimatorControllerOverrides>();
    private AnimatorControllerOverrides currentControllerOverride;
    private int currentOverrideIndex = 0;

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

    public void SelectController(Animator animator)
    {
        int num = Random.Range(0, animatorControllers.Count);

        AnimatorControllerOverrides controller = animatorControllers[num];

        while (controller == currentControllerOverride)
        {
            num = Random.Range(0, animatorControllers.Count);
            controller = animatorControllers[num];
        }

        currentOverrideIndex = num;

        animator.runtimeAnimatorController = controller.controller;
    }

    private AnimatorOverrideController RandomOverride()
    {
        AnimatorControllerOverrides temp = animatorControllers[currentOverrideIndex];

        int rand = Random.Range(0, temp.Overrides.Count);

        return temp.Overrides[rand];
    }

    public AnimatorOverrideController SetOverrideController()
    {
        return RandomOverride();
    }

    //public AnimatorOverrideController SetOverrideController(Animator animator)
    //{
    //    return RandomController(animator);
    //}


    // apply controller on spawn 
    // hold reference to that controller and set override from that controller

}

[System.Serializable]
public class AnimatorControllerOverrides
{
    public RuntimeAnimatorController controller;
    public List<AnimatorOverrideController> Overrides = new List<AnimatorOverrideController>();
}