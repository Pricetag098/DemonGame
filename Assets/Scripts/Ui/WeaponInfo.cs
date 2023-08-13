using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WeaponInfo : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI ammoText;
	[SerializeField] TextMeshProUGUI nameText;
	[SerializeField] Holster holster;

	private void Awake()
	{
		holster = FindObjectOfType<Holster>();
	}
	// Update is called once per frame
	void Update()
    {
		
        ammoText.text = holster.HeldGun.ammoLeft.ToString() + "/" + holster.HeldGun.stash;
		nameText.text = holster.HeldGun.gunName;
    }
}
