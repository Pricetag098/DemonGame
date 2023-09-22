using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableUnityEvent : Interactable
{
	public UnityEvent unityEvent;
	public string interactMessage;
	[SerializeField] private bool hasInteractMessage;
	public override void Interact(Interactor interactor)
	{
		unityEvent.Invoke();
	}

	public override void StartHover(Interactor interactor)
	{
		base.StartHover(interactor);
		if (hasInteractMessage)
		{
			interactor.display.DisplayMessage(false, interactMessage);
		}
	}
	public override void EndHover(Interactor interactor)
	{
		base.EndHover(interactor);
		if (hasInteractMessage)
		{
			interactor.display.HideText();
		}
	}
}
