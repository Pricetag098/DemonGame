using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAbilityCaster : MonoBehaviour
{
    [HideInInspector]public AbilityCaster caster;
    public int activeIndex;
    public InputActionProperty useAction;
    public InputActionProperty swapAction;
    // Start is called before the first frame update
    void Start()
    {
        caster = GetComponent<AbilityCaster>();
        swapAction.action.performed += Swap;
    }

	private void OnEnable()
	{
		useAction.action.Enable();
	}

	private void OnDisable()
	{
		useAction.action.Disable();
	}

	// Update is called once per frame
	void Update()
    {
        if ((useAction.action.IsPressed() && caster.abilities[activeIndex].castMode == Ability.CastModes.hold) ||
            (useAction.action.WasPerformedThisFrame() && caster.abilities[activeIndex].castMode == Ability.CastModes.press) ||
             caster.abilities[activeIndex].castMode == Ability.CastModes.passive)
        {
            caster.Cast(activeIndex, Camera.main.transform.position, Camera.main.transform.forward);
        }
    }

    void Swap(InputAction.CallbackContext context)
    {
        caster.abilities[activeIndex].DeSelect();
        activeIndex++;
        if(activeIndex > caster.abilities.Length)
        {
            activeIndex = 0;
        }
    }
}
