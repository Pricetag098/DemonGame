using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathStateToggler : MonoBehaviour
{
    [SerializeField] bool deathState;

    public void Toggle(bool alive)
    {
        gameObject.SetActive(alive && deathState);
    }
}
