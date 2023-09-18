using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathStateToggler : MonoBehaviour
{
    [SerializeField] bool activeOnAlive;

    public void Toggle(bool alive)
    {
        gameObject.SetActive(alive == activeOnAlive);
    }
}
