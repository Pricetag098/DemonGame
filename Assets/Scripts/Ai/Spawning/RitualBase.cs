using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualBase : Interactable
{
    [SerializeField] protected string activateMessage = "To Activate ";

    protected SpawnerManager manager;
    protected RitualSpawner spawner;

    private void Awake()
    {
        manager = FindObjectOfType<SpawnerManager>();
        spawner = GetComponent<RitualSpawner>();

        OnAwake();
    }

    public override void Interact(Interactor interactor)
    {
        
    }

    public override void EndHover(Interactor interactor)
    {
        base.EndHover(interactor);
        interactor.display.HideText();
    }
    public override void StartHover(Interactor interactor)
    {
        base.StartHover(interactor);
        if (spawner.ritualComplete == false && spawner.RitualActive == false)
        {
            interactor.display.DisplayMessage(true, activateMessage, null);
        }
    }

    public virtual void OnAwake()
    {

    }
}
