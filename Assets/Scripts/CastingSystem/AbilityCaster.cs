using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class AbilityCaster : MonoBehaviour
{
    Ability emptyAbility;
    public float blood;
    public float maxBlood;
    public Ability[] abilities;

    const string BaseAbilityPath = "Abilities/Empty";
    [Tooltip("For visualiser")]
    public Transform castOrigin;

    // Start is called before the first frame update
    void Awake()
    {
        if (abilities.Length == 0)
            return;
        emptyAbility = Resources.Load<Ability>(BaseAbilityPath);
        for (int i = 0; i < abilities.Length; i++)
        {
            if(abilities[i] == null)
			{
                abilities[i] = Instantiate(emptyAbility);
			}
			else
			{
                abilities[i] = Instantiate(abilities[i]);
			}
            abilities[i].Equip(this);
        }
    }
	

	// Update is called once per frame
	void Update()
    {
        if(abilities.Length == 0)
            return;
        for(int i = 0; i < abilities.Length; i++)
		{
            abilities[i].Tick();
		}
		
    }

    public void Cast(int index,Vector3 origin,Vector3 direction)
	{
        if (blood < abilities[index].bloodCost)
            return;
       
        abilities[index].Cast(origin, direction);
    }

    public void AddBlood(float amount)
	{
        blood = Mathf.Clamp(blood + amount, 0, maxBlood);
	}

    public bool HasAbility(Ability ability)
	{
        foreach(Ability item in abilities)
		{
            if(item.abilityName == ability.abilityName)
                return true;
		}
        return false;
	}

    public void SetAbility(int index,Ability ability)
	{
        for(int i = 0; i < abilities.Length; i++)
		{
            if(abilities[i] == emptyAbility)
			{
                index = i;
			}
		}
        if (abilities[index] != null)
            abilities[index].DeEquip();
        abilities[index] = ability;
        ability.Equip(this);
	}
}
