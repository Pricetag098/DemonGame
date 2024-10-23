using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class CheckForAnyGamepads : MonoBehaviour
{
    EventSystem eventSystem;
    bool controllerConnected = false;

    [SerializeField] GameObject selected;
    [SerializeField] InputActionProperty inputAction;
    private void Awake()
    {
        eventSystem = GetComponent<EventSystem>();
        inputAction.action.performed += CheckControllerStatus;
        inputAction.action.Enable();
    }

    public void CheckControllerStatus(InputAction.CallbackContext context)
    {
        if (Gamepad.all.Count > 0)
        {
            controllerConnected = true;

            eventSystem.SetSelectedGameObject(selected);

            inputAction.action.Disable();
        }
    }

    private void Update()
    {
        if (Gamepad.all.Count == 0)
        {
            controllerConnected = false;

            eventSystem.SetSelectedGameObject(null);

            inputAction.action.Enable();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (Gamepad.all.Count > 0)
        {
            controllerConnected = true;

            eventSystem.SetSelectedGameObject(selected);

            inputAction.action.Disable();
        }
        else
        {
            if (Gamepad.all.Count == 0)
            {
                controllerConnected = false;

                eventSystem.SetSelectedGameObject(null);

                inputAction.action.Enable();
            }
        }
    }
}
