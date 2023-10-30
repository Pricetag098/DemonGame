using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventFunction : MonoBehaviour
{
    public UnityEvent unityEvent;

    public void CallEvents()
    {
        unityEvent.Invoke();
    }
}
