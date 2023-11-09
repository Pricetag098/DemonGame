using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WheelCastTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    AbilitySlot slot;

    private void Start()
    {
        slot = GetComponentInParent<AbilitySlot>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Cursor Entering " + name + " GameObject");
        slot.OnSelect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Cursor Exiting " + name + " GameObject");
        slot.OnDeselect();
    }
}
