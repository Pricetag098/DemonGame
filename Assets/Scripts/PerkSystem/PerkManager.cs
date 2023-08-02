using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoBehaviour
{

    public List<Perk> perkList;
    public delegate void TickPerk();
    public TickPerk tickPerk;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Perk perk in perkList)
		{
            perk.Equip(this);
		}
        GetComponent<Health>().OnDeath += ClearPerks;
    }

    public void AddPerk(Perk perk)
	{
        if (perkList.Contains(perk))
            return;
        Debug.Log(perk);
        perk.Equip(this);
        perkList.Add(perk);
	}

	private void Update()
	{
        if(tickPerk != null)
        tickPerk();
	}

	public void RemovePerk(Perk perk)
	{
        perk.UnEquip();
        perkList.Remove(perk);
	}

    public void ClearPerks()
	{
        foreach(Perk perk in perkList)
		{
            perk.UnEquip();
		}
        perkList.Clear();
	}

    public bool HasPerk(Perk perk)
	{
        foreach(Perk p in perkList)
		{
            if(p.perkName == perk.perkName)
                return true;
		}
        return false;
	}

	private void OnDestroy()
	{
        if(GetComponent<Health>()!=null)
        GetComponent<Health>().OnDeath -= ClearPerks;
        ClearPerks();
	}
}
