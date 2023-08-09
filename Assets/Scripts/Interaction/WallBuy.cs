using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuy : ShopInteractable
{
    [SerializeField] Holster holster;
	[SerializeField] GameObject prefab;
	
	

	protected override bool CanBuy()
	{
		return true;
	}

	protected override void DoBuy()
	{
		Gun g;
		if (holster.HasGun(prefab.GetComponent<Gun>(),out g))
		{
			g.AddToStashPercent(1);
		}
		else
		{
            GameObject gun = Instantiate(prefab, holster.transform);
            holster.HeldGun = gun.GetComponent<Gun>();
        }
		
	}
}
