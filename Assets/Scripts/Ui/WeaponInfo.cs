using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class WeaponInfo : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI ammoText;
    [SerializeField] RectTransform nameParent;
    [SerializeField] RectTransform reserveNameParent;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI reserveNameText;
    [SerializeField] Holster holster;
	bool reserveOn = false;

    public AnimationCurve curve;
    public float duration;

	private void Awake()
	{
		holster = FindObjectOfType<Holster>();

		WeaponSwitchTween();
	}
	// Update is called once per frame
	void Update()
    {
        ammoText.text = holster.HeldGun.ammoLeft.ToString() + "/" + holster.HeldGun.stash;
		nameText.text = holster.HeldGun.gunName;
    }

	public void WeaponSwitchTween()
	{
		reserveNameParent.DOAnchorPos(new Vector2(-30, 40), holster.HeldGun.drawTime).SetEase(curve);
        reserveNameText.fontSize = 50;
        reserveNameText.color = Color.white;

        nameParent.DOAnchorPos(new Vector2(0, 90), holster.HeldGun.drawTime).SetEase(curve);
        nameText.fontSize = 45;
        reserveNameText.color = Color.grey;
    }
}
