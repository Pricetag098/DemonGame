using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableUnityEvent : Interactable
{
	public UnityEvent unityEvent;
	public string interactMessage;
	public override void Interact(Interactor interactor)
	{
		unityEvent.Invoke();
	}

	public override void StartHover(Interactor interactor)
	{
		base.StartHover(interactor);
		interactor.display.DisplayMessage(false,interactMessage);
	}
	public override void EndHover(Interactor interactor)
	{
		base.EndHover(interactor);
		interactor.display.HideText();
	}
}
