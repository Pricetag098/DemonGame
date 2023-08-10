using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InteractionDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI messageText;
	CanvasGroup canvasGroup;
	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}
	public void DisplayMessage(bool canInteract, string text)
	{
		interactText.enabled = canInteract;
		icon.enabled = canInteract;
		messageText.text = text;
		canvasGroup.alpha = 1f;
	}
	public void HideText()
	{
		interactText.enabled = false;
		icon.enabled = false;
		canvasGroup.alpha = 0f;
	}
}
