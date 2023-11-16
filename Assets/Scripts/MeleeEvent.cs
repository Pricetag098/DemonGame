using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEvent : MonoBehaviour
{
    [SerializeField] GameObject bloodKnife;

    private void Start()
    {
        bloodKnife.SetActive(false);
    }

    public void KnifeOn()
    {
        bloodKnife.SetActive(true);
    }

    public void KnifeOff()
    {
        bloodKnife.SetActive(false);
    }
}
