using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAbilityCaster : MonoBehaviour,IDataPersistance<GameData>,IDataPersistance<SessionData>
{
    [HideInInspector]public AbilityCaster caster;
    public int activeIndex;
    public InputActionProperty useAction;
    public InputActionProperty swapAction;
	public InputActionProperty setAbility1Action;
	public InputActionProperty setAbility2Action;
	public InputActionProperty setAbility3Action;

	public float bloodSpent = 0;
    public float bloodGained = 0;

    public Ability ActiveAbility { get { return caster.abilities[activeIndex]; } set { SetAbility(value); } }
	// Start is called before the first frame update

	private void Awake()
	{
        caster = GetComponent<AbilityCaster>();
        swapAction.action.performed += Swap;
        setAbility1Action.action.performed += SelectAbility1;
		setAbility2Action.action.performed += SelectAbility2;
		setAbility3Action.action.performed += SelectAbility3;
	}
	void Start()
    {
        
        
        caster.OnAddBlood += OnAddBlood;
        caster.OnRemoveBlood += OnRemoveBlood;
        
    }

	private void OnEnable()
	{
		useAction.action.Enable();
        swapAction.action.Enable();
        setAbility1Action.action.Enable();
        setAbility2Action.action.Enable();
        setAbility3Action.action.Enable();
	}

	private void OnDisable()
	{
		useAction.action.Disable();
        swapAction.action.Disable();
        setAbility1Action.action.Disable();
        setAbility2Action.action.Disable();
        setAbility3Action.action.Disable();
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
        //bloodGained = data.bloodGained;
        //bloodSpent = data.bloodSpent;
	}

    void IDataPersistance<GameData>.SaveData(ref GameData data)
	{
        data.bloodGained += bloodGained;
        data.bloodSpent += bloodSpent;
	}

    void Swap(InputAction.CallbackContext context)
    {
        int lastActiveIndex = activeIndex;

        bool done = false;
        while(caster.abilities[activeIndex].guid == caster.emptyAbility.guid || ! done)
		{
            done = true;
            activeIndex++;
            if (activeIndex > caster.abilities.Length - 1)
            {
                activeIndex = 0;
            }
        }

        caster.abilities[lastActiveIndex].DeSelect();
        
        caster.abilities[activeIndex].Select();
    }

    void SelectAbility1(InputAction.CallbackContext context)
    {
        SelectAbility(0);
    }
	void SelectAbility2(InputAction.CallbackContext context)
	{
		SelectAbility(1);
	}
	void SelectAbility3(InputAction.CallbackContext context)
	{
		SelectAbility(2);
	}


	void SelectAbility(int index)
    {
        if (index == activeIndex)
            return;
        if(caster.abilities[index].guid != caster.emptyAbility.guid)
        {
			caster.abilities[activeIndex].DeSelect();
            activeIndex = index;
			caster.abilities[activeIndex].Select();
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

    public void LoadData(SessionData data)
    {
        //throw new System.NotImplementedException();
    }

    public void SaveData(ref SessionData data)
    {
        data.bloodSpent = bloodSpent;
        data.bloodGained = bloodGained;
    }
}
