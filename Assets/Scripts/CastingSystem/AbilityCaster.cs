using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class AbilityCaster : MonoBehaviour
{
    public float blood;
    public float maxBlood;
    public Ability[] abilities;
    public int activeIndex;
    public InputActionProperty useAction;

    [Tooltip("For visualiser")]
    public Transform castOrigin;
    // Start is called before the first frame update
    void Start()
    {
        if (abilities.Length == 0)
            return;
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].Equip(this);
        }
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
        if(abilities.Length == 0)
            return;
        for(int i = 0; i < abilities.Length; i++)
		{
            abilities[i].Tick(i == activeIndex);
		}
		if ((useAction.action.IsPressed() && abilities[activeIndex].castMode == Ability.CastModes.hold) ||
            (useAction.action.WasPerformedThisFrame() && abilities[activeIndex].castMode == Ability.CastModes.press) || 
            abilities[activeIndex].castMode == Ability.CastModes.passive)
		{
            abilities[activeIndex].Cast();

        }
    }

    public void AddBlood(float amount)
	{
        blood = Mathf.Clamp(blood + amount, 0, maxBlood);
	}
}
