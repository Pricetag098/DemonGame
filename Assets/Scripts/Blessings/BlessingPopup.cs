using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlessingPopup : MonoBehaviour
{
	CanvasGroup canvasGroup;
	[SerializeField] TextMeshProUGUI icon;
	[SerializeField] TextMeshProUGUI title;
	[SerializeField] float entryTime;
	[SerializeField] float holdTime;
	[SerializeField] float punchScale;
	RectTransform rectTransform;
	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		rectTransform = GetComponent<RectTransform>();
	}

	public void Display(Blessing blessing)
	{
		DOTween.Kill(this,true);
		icon.text = blessing.blessingFontRef;
		title.text = blessing.blessingName;

		Sequence sequence = DOTween.Sequence(this);
		sequence.Append(canvasGroup.DOFade(1, entryTime));
		sequence.Join(rectTransform.DOPunchScale(Vector3.one * punchScale, entryTime));
		sequence.AppendInterval(holdTime);
		sequence.Append(canvasGroup.DOFade(0,entryTime));
	}
}
