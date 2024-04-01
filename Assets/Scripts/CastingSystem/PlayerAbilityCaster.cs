using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAbilityCaster : MonoBehaviour,IDataPersistance<GameData>,IDataPersistance<SessionData>
{
    [HideInInspector]public AbilityCaster caster;
    public int activeIndex;
    public int previousActiveIndex;
    public int upgradeNum = 0;
    int newActiveIndex;
    public InputActionProperty useAction;
    public InputActionProperty swapAction;
	public InputActionProperty setAbility1Action;
	public InputActionProperty setAbility2Action;
	public InputActionProperty setAbility3Action;
	public InputActionProperty setAbility4Action;
	public InputActionProperty setAbility5Action;
	public InputActionProperty setAbility6Action;
    public InputActionProperty quickSwap;

	public float bloodSpent = 0;
    public float bloodGained = 0;

    WeaponWheel abilityWheel;
    PlayerDeath playerDeath;
	enum State
	{
		normal,
		drawing,
		holstering,
		replacing
	}
	[SerializeField] State state;

    float timer;

	public Ability ActiveAbility { get { return caster.abilities[activeIndex]; } set { SetAbility(value); } }
	// Start is called before the first frame update

	private void Awake()
	{
        playerDeath = GetComponent<PlayerDeath>();
        caster = GetComponent<AbilityCaster>();
        swapAction.action.performed += Swap;
        setAbility1Action.action.performed += SelectAbility1;
		setAbility2Action.action.performed += SelectAbility2;
		setAbility3Action.action.performed += SelectAbility3;
		setAbility4Action.action.performed += SelectAbility4;
		setAbility5Action.action.performed += SelectAbility5;
		setAbility6Action.action.performed += SelectAbility6;
		quickSwap.action.performed += QuickSwapAbility;
	}
	void Start()
    {
        abilityWheel = FindObjectOfType<WeaponWheel>();
        
        caster.OnAddBlood += OnAddBlood;
        caster.OnRemoveBlood += OnRemoveBlood;
		caster.animator.runtimeAnimatorController = ActiveAbility.controller;

		DrawAbility();

	}

	private void OnEnable()
	{
		useAction.action.Enable();
        swapAction.action.Enable();
        setAbility1Action.action.Enable();
        setAbility2Action.action.Enable();
        setAbility3Action.action.Enable();
        setAbility4Action.action.Enable();
        setAbility5Action.action.Enable();
        setAbility6Action.action.Enable();
        quickSwap.action.Enable();

	}

	private void OnDisable()
	{
		useAction.action.Disable();
        swapAction.action.Disable();
        setAbility1Action.action.Disable();
        setAbility2Action.action.Disable();
        setAbility3Action.action.Disable();
        setAbility4Action.action.Disable();
        setAbility5Action.action.Disable();
        setAbility6Action.action.Disable();
        quickSwap.action.Disable();
    }

    bool IsLocked()
    {
        bool locked = playerDeath.dead;
        return locked || Time.timeScale == 0;
    }

	// Update is called once per frame
	void Update()
    {
        timer-= Time.deltaTime;
        switch (state)
        {
            case State.normal:
                if (IsLocked())
                    break;
                if (((useAction.action.IsPressed() && caster.abilities[activeIndex].castMode == Ability.CastModes.hold) ||
			(useAction.action.WasPerformedThisFrame() && caster.abilities[activeIndex].castMode == Ability.CastModes.press) ||
			 caster.abilities[activeIndex].castMode == Ability.CastModes.passive))
				{
					caster.Cast(activeIndex, Camera.main.transform.position, Camera.main.transform.forward);
				}
				break;

            case State.holstering:
                if(timer < 0)
                {
                    ActiveAbility.EndDeSelect();
                    activeIndex = newActiveIndex;
                    caster.animator.runtimeAnimatorController = ActiveAbility.controller;

                    DrawAbility();
                }
                break;

            case State.drawing:
                if(timer< 0)
                {
                    state = State.normal;
                    ActiveAbility.EndSelect();
                }
                break;
            case State.replacing:
                if(timer< 0)
                {
                    caster.SetAbility(activeIndex, replacingAbility);
					caster.animator.runtimeAnimatorController = ActiveAbility.controller;

					DrawAbility();
				}
                break;
        }

        caster.UpdateAbilitys(Camera.main.transform.position, Camera.main.transform.forward);
        
    }

    public void OnUpgrade()
    {
        upgradeNum++;
        abilityWheel.UpdateWheel(upgradeNum);
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
  //      int lastActiveIndex = activeIndex;

  //      bool done = false;
  //      while(caster.abilities[activeIndex].guid == caster.emptyAbility.guid || ! done)
		//{
  //          done = true;
  //          activeIndex++;
  //          if (activeIndex > caster.abilities.Length - 1)
  //          {
  //              activeIndex = 0;
  //          }
  //      }

  //      caster.abilities[lastActiveIndex].DeSelect();
        
  //      caster.abilities[activeIndex].Select();
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
    void SelectAbility4(InputAction.CallbackContext context)
    {
        SelectAbility(3);
    }
    void SelectAbility5(InputAction.CallbackContext context)
    {
        SelectAbility(4);
    }
    void SelectAbility6(InputAction.CallbackContext context)
    {
        SelectAbility(5);
    }
    void QuickSwapAbility(InputAction.CallbackContext context)
    {
        SelectAbility(previousActiveIndex);
    }


    public void SelectAbility(int index)
    {
        if (index == activeIndex || state != State.normal)
            return;
        if(caster.abilities[index].guid != caster.emptyAbility.guid)
        {
            previousActiveIndex = activeIndex;
            newActiveIndex = index;
			HolsterAbility();
            
		}
        
    }


    public void SetAbility(Ability ability)
    {
        caster.SetAbility(ability.abilitySlot, ability);
        SelectAbility(ability.abilitySlot);
        newActiveIndex = ability.abilitySlot;
        /*        for(int i = 0; i < caster.abilities.Length; i++)
                {
                    if(ability.guid == caster.abilities[i].guid)
                    {

                        if(i != activeIndex)
                        {
                            caster.SetAbility(i, ability);
                            HolsterAbility();
                            newActiveIndex = i;
                        }
                        else
                        {
                            ReplaceAbility(ability);
                        }
                        return;
                    }
                    else if (caster.abilities[i].guid == caster.emptyAbility.guid)
                    {

                        if (i != activeIndex)
                        {
                            caster.SetAbility(i, ability);
                            HolsterAbility();
                            newActiveIndex = i;
                        }
                        else
                        {
                            ReplaceAbility(ability);
                        }
                        return;
                    }
                }
                ReplaceAbility(ability);*/
    }

    Ability replacingAbility;
    void ReplaceAbility(Ability ability)
    {
        replacingAbility = ability;
        state = State.replacing;
		ActiveAbility.StartDeSelect();
		caster.animator.SetFloat("UnEquipSpeed", 1 / ActiveAbility.holsterTime);
		caster.animator.SetTrigger("Unequip");
		timer = ActiveAbility.holsterTime;
	}


    void DrawAbility()
    {
        state = State.drawing;
        ActiveAbility.StartSelect();
        caster.animator.SetFloat("EquipSpeed", 1 / ActiveAbility.drawTime);
        timer = ActiveAbility.drawTime;
    }

    void HolsterAbility()
    {
        state = State.holstering;
        ActiveAbility.StartDeSelect();
        caster.animator.SetFloat("UnEquipSpeed", 1 / ActiveAbility.holsterTime);
        caster.animator.SetTrigger("Unequip");
        timer = ActiveAbility.holsterTime;
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
