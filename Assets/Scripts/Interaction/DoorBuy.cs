using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorBuy : ShopInteractable
{
    public bool open;
    [SerializeField] string animationTrigger = "Open";
    [SerializeField] Optional<UnityEvent> interactEvents;
    [SerializeField] Optional<Animator> doorAnimator;
    [SerializeField] Optional<List<ArcaneLock>> arcaneLock;

    [SerializeField] Optional<Area> AreaConnection1;
    [SerializeField] Optional<Area> AreaConnection2;
    [SerializeField] List<Optional<Area>> AreaConnections = new List<Optional<Area>>();

    private DetectArea DetectArea;
    private Spawners spawners;

    private void Awake()
    {
        DetectArea = FindObjectOfType<DetectArea>();
    }

    protected override bool CanBuy(Interactor interactor)
    {
        return !open;
    }
    protected override void DoBuy(Interactor interactor)
    {
        open = true;

        foreach(Optional<Area> area in AreaConnections)
        {
            if(area.Enabled)
            {
                area.Value.discovered = true;

                Spawners.GetDictionaryArea(DetectArea.CurrentArea, out Area currentArea);

                foreach (Optional<AreaConnect> areasInConnections in area.Value.OptionalAreas)
                {
                    if (areasInConnections.Enabled)
                    {
                        if (areasInConnections.Value.Area == currentArea)
                        {
                            areasInConnections.Value.Open = true;

                            foreach (Optional<AreaConnect> AreasTouchingCurrentArea in areasInConnections.Value.Area.OptionalAreas) // all adjacent areas in main gate
                            {
                                if (AreasTouchingCurrentArea.Value.Area == AreaConnection1.Value) // if main gate contains courtyard
                                {
                                    AreasTouchingCurrentArea.Value.Open = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        GetComponent<Collider>().enabled = false;   
        if (doorAnimator.Enabled)
        {
            doorAnimator.Value.SetTrigger(animationTrigger);
            if (arcaneLock.Enabled)
            {
                for (int i = 0; i < arcaneLock.Value.Count; i++)
                {
                    arcaneLock.Value[i].DisolveLock();
                }
            }
        }
        else
        {
            transform.parent.gameObject.SetActive(false);
        }
        if (interactEvents.Enabled)
        {
            interactEvents.Value.Invoke();
        }
    }

    protected override void CantBuy(Interactor interactor)
    {
        if (arcaneLock.Enabled)
        {
            for (int i = 0; i < arcaneLock.Value.Count; i++)
            {
                arcaneLock.Value[i].NoBuy();
            }
        }
    }
}
