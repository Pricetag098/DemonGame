using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WeaponInfo : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI clipAmountText;
	[SerializeField] TextMeshProUGUI stashAmountText;
	[SerializeField] TextMeshProUGUI dividerText;
	[SerializeField] TextMeshProUGUI nameText;
	[SerializeField] Holster holster;

	[SerializeField] Color defaultColour;
	[SerializeField] Color noAmmoColour;

	private void Awake()
	{
		holster = FindObjectOfType<Holster>();
	}
	// Update is called once per frame
	void Update()
    {

		nameText.text = holster.HeldGun.gunName;
		clipAmountText.text = holster.HeldGun.ammoLeft.ToString();
		stashAmountText.text = holster.HeldGun.stash.ToString();
		clipAmountText.color = defaultColour;
		stashAmountText.color = defaultColour;
		dividerText.color = defaultColour;
		if(holster.HeldGun.ammoLeft == 0)
        {
			clipAmountText.color = noAmmoColour;
		}
		if(holster.HeldGun.stash == 0)
        {
			stashAmountText.color = noAmmoColour;
		}
		if(holster.HeldGun.ammoLeft == 0 && holster.HeldGun.stash == 0)
        {
			dividerText.color = noAmmoColour;
		}
		
    }
}
