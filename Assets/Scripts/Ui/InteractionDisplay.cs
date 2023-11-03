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
	CanvasGroup canvasGroup;
	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.alpha = 0;
	}
	public void DisplayMessage(bool canInteract, string text)
	{
		canvasGroup.DOFade(1, 0.2f);
		interactText.enabled = canInteract;
		icon.enabled = canInteract;
		messageText.text = text;
	}
	public void HideText()
	{
        canvasGroup.DOFade(0, 0.2f).OnComplete(() => 
		{
            interactText.enabled = false;
            icon.enabled = false;
        });

    }
}
