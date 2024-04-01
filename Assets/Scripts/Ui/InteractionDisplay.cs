using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class InteractionDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI messageText;
	[SerializeField] TextMeshProUGUI costText;

	[SerializeField] Vector3 cantBuyPunchScale;
    [SerializeField] int punchVibrado = 10;
    [SerializeField] float punchElasticity = 1;
    [SerializeField] float cantBuyPunchTime;
    [SerializeField] Color cantBuyColour;

	CanvasGroup canvasGroup;
	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.alpha = 0;
	}
	public void DisplayMessage(bool canInteract, string displayText, string costAmount)
	{
		DOTween.Kill(canvasGroup);
		canvasGroup.DOFade(1, 0.2f);
		interactText.enabled = canInteract;
		icon.enabled = canInteract;
		messageText.text = displayText;
		if(costAmount == null)
        {
			costText.text = "";
			return;

		}
		costText.text = costAmount;
	}
	public void HideText()
	{
        canvasGroup.DOFade(0, 0.2f).OnComplete(() => 
		{
            interactText.enabled = false;
            icon.enabled = false;
        });

    }

	public void CantBuy()
	{
		DOTween.Kill(this);
		Color originalColour = interactText.color;
		interactText.color = cantBuyColour;
		messageText.color = cantBuyColour;
		Sequence punch = DOTween.Sequence();
        punch.Append(interactText.transform.parent.DOPunchScale(cantBuyPunchScale, cantBuyPunchTime, punchVibrado, punchElasticity));
		punch.AppendCallback(() => { interactText.color = originalColour; messageText.color = originalColour; });
    }
}
