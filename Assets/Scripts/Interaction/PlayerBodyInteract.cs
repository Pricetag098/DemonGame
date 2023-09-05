using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBodyInteract : Interactable
{
    public string interactMessage;
    public GameObject body;

    public void Show()
    {
        body.SetActive(true);
    }
    PlayerDeath death;
    private void Awake()
    {
        death = FindObjectOfType<PlayerDeath>();
    }

    private void Start()
    {
        body.SetActive(false);
    }

    public override void Interact(Interactor interactor)
    {
        death.ReturnToBody();
        body.SetActive(false);
    }

    public override void StartHover(Interactor interactor)
    {
        base.StartHover(interactor);
        interactor.display.DisplayMessage(true, interactMessage);
    }
    public override void EndHover(Interactor interactor)
    {
        base.EndHover(interactor);
        interactor.display.HideText();
    }
}
