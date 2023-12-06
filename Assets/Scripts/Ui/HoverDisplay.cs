using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverDisplay : Interactable
{
    public string text;
    public override void StartHover(Interactor interactor)
    {
        base.StartHover(interactor);
        interactor.display.DisplayMessage(false, text, "");
    }

    public override void EndHover(Interactor interactor)
    {
        base.EndHover(interactor);
        interactor.display.HideText();
    }
}
