using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootEvent : MonoBehaviour
{
    Health health;

    public UnityEvent events;


    private void Awake()
    {
        health = GetComponent<Health>();

        health.OnHit += OnHit;
    }

    private void OnHit()
    {
        events.Invoke();
    }
}
