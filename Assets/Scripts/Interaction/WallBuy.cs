using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuy : ShopInteractable
{
    Holster holster;
	[SerializeField] GameObject prefab;
	
	private void Start()
	{
		holster = FindObjectOfType<Holster>();
	}

	protected override bool CanBuy()
	{
		return true;
	}

	protected override void DoBuy()
	{
		GameObject gun = Instantiate(prefab,holster.transform);
		holster.HeldGun = gun.GetComponent<Gun>();
	}
}
