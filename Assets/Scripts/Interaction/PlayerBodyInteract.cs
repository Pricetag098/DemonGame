using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBodyInteract : MonoBehaviour
{
    public string interactMessage;
    public GameObject body;

    public void Show()
    {
        body.SetActive(true);
    }

    public void Hide()
    {
        body.SetActive(false);
    }
    PlayerDeath death;
    private void Awake()
    {
        death = FindObjectOfType<PlayerDeath>();
    }

    private void Start()
    {
        body.SetActive(false);
    }

	private void OnTriggerEnter(Collider other)
	{
        death.ReturnToBody();
	}
}
