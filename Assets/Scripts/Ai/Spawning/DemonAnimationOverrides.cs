using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAnimationOverrides : MonoBehaviour
{
    [Header("Animation Overwrite")]
    [SerializeField] private List<AnimatorControllerOverrides> animatorControllers = new List<AnimatorControllerOverrides>();
    private AnimatorControllerOverrides currentControllerOverride;
    private int currentOverrideIndex = 0;

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
}

[System.Serializable]
public class AnimatorControllerOverrides
{
    public RuntimeAnimatorController controller;
    public List<AnimatorOverrideController> Overrides = new List<AnimatorOverrideController>();
}