using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class AbilityCaster : MonoBehaviour
{
    [HideInInspector]
    public Ability emptyAbility;
    public float blood;
    public float maxBlood;
    public Ability[] abilities;

    const string BaseAbilityPath = "Abilities/Empty";
    [Tooltip("For visualiser")]
    public Transform castOrigin;

    public Optional<PlayerStats> playerStats;

    public delegate void CollisionAction(Collision collision);
    public CollisionAction collisionAction;
    public float DamageMulti
    {
        get { if (playerStats.Enabled)
                return playerStats.Value.abilityDamageMulti;
        else return 1f;
        }
    }
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

    public delegate void Action(float amount);
    public Action OnAddBlood;
    public Action OnRemoveBlood;
    public void AddBlood(float amount)
	{
        if(amount + blood > maxBlood)
		{
            amount = maxBlood - blood;
		}
        blood = Mathf.Clamp(blood + amount, 0, maxBlood);
        if(OnAddBlood != null)
            OnAddBlood(amount);
	}

    public void RemoveBlood(float amount)
	{
        blood = Mathf.Clamp(blood - amount, 0, maxBlood);
        if(OnRemoveBlood != null)
            OnRemoveBlood(amount);
    }

    public bool HasAbility(Ability ability)
	{
        foreach(Ability item in abilities)
		{
            if(item.guid == ability.guid)
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
	private void OnCollisionEnter(Collision collision)
	{
        if(collisionAction != null)
        collisionAction(collision);
	}
}
