using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAbilityCaster : MonoBehaviour,IDataPersistance<GameData>
{
    [HideInInspector]public AbilityCaster caster;
    public int activeIndex;
    public InputActionProperty useAction;
    public InputActionProperty swapAction;

    public float bloodSpent = 0;
    public float bloodGained = 0;

    public Ability ActiveAbility { get { return caster.abilities[activeIndex]; } set { SetAbility(value); } } 
    // Start is called before the first frame update
    void Start()
    {
        caster = GetComponent<AbilityCaster>();
        swapAction.action.performed += Swap;
        caster.OnAddBlood += OnAddBlood;
        caster.OnRemoveBlood += OnRemoveBlood;
    }

	private void OnEnable()
	{
		useAction.action.Enable();
        swapAction.action.Enable();
	}

	private void OnDisable()
	{
		useAction.action.Disable();
        swapAction.action.Disable();
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

    void OnAddBlood(float amount)
	{
        bloodGained += amount;
	}

    void OnRemoveBlood(float amount)
	{
        bloodSpent += amount;
	}

    void IDataPersistance<GameData>.LoadData(GameData data)
	{
        bloodGained = data.bloodGained;
        bloodSpent = data.bloodSpent;
	}

    void IDataPersistance<GameData>.SaveData(ref GameData data)
	{
        data.bloodGained = bloodGained;
        data.bloodSpent = bloodSpent;
	}

    void Swap(InputAction.CallbackContext context)
    {
        caster.abilities[activeIndex].DeSelect();
        activeIndex++;
        if(activeIndex > caster.abilities.Length-1)
        {
            activeIndex = 0;
        }
    }

    public void SetAbility(Ability ability)
    {
        for(int i = 0; i < caster.abilities.Length; i++)
        {
            if(caster.abilities[i].GetType() == typeof(Ability))
            {
                caster.SetAbility(i, ability);
                return;
            }
        }
        caster.SetAbility(activeIndex, ability);
    }
}
