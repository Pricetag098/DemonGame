using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBoxInteractable : Interactable
{
    [SerializeField] protected string activateMessage = "To Activate SoulBox ";

    [SerializeField] SoulBox soulBox;
    public override void Interact(Interactor interactor)
    {
        soulBox.active = true;
    }
    public override void EndHover(Interactor interactor)
    {
        base.EndHover(interactor);
        interactor.display.HideText();
    }
    public override void StartHover(Interactor interactor)
    {
        base.StartHover(interactor);
        if (soulBox.active == false) { interactor.display.DisplayMessage(true, activateMessage); }
    }
}
