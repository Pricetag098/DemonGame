using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAbilityCaster : MonoBehaviour
{
    [HideInInspector]public AbilityCaster caster;
    public int activeIndex;
    public InputActionProperty useAction;
    // Start is called before the first frame update
    void Start()
    {
        caster = GetComponent<AbilityCaster>();
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
}
