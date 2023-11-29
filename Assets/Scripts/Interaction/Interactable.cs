using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SelectionBase]
public class Interactable : MonoBehaviour
{
	public bool hovered;

    public virtual void Interact(Interactor interactor)
	{

	}

	public virtual void StartHover(Interactor interactor)
	{
		if (hovered) return;
		hovered = true;
		
	}

	public virtual void EndHover(Interactor interactor)
	{
		hovered = false;
	}
}
