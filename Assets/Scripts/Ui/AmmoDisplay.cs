using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AmmoDisplay : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI;
    [SerializeField] Holster holster;

	private void Start()
	{
		textMeshProUGUI = GetComponent<TextMeshProUGUI>();
	}
	// Update is called once per frame
	void Update()
    {
        textMeshProUGUI.text = holster.HeldGun.ammoLeft.ToString() + "/" + holster.HeldGun.stash;
    }
}
