using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float interactionRange;

    bool hasInteractor;
    Interactable lastInteractor;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward,out hit, interactionRange, layerMask))
		{
            Interactable interactable;
            if(hit.collider.TryGetComponent(out interactable))
			{
                if(lastInteractor != interactable)
				{
                    hasInteractor = true;
                    lastInteractor = interactable;
                    interactable.StartHover();
				}
			}
			else
			{
                if(hasInteractor)
                    lastInteractor.EndHover();
                hasInteractor=false;
			}
		}
		else
		{
            if (hasInteractor)
                lastInteractor.EndHover();
            hasInteractor=false;
        }
    }
}
