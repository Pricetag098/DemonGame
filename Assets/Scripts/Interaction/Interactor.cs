using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
public class Interactor : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float interactionRange;
    [SerializeField] InputActionProperty inputAction;
    public bool hasInteractable;
	public Interactable interactable;
	public InteractionDisplay display;
	[HideInInspector] public PlayerStats playerStats;
	[HideInInspector] public Holster holster;
	[HideInInspector] public PlayerAbilityCaster caster;
	[HideInInspector] public PerkManager perkManager;

	private void Start()
	{
        inputAction.action.performed += Interact;
		playerStats = GetComponent<PlayerStats>();
		caster = GetComponent<PlayerAbilityCaster>();
		holster = GetComponentInChildren<Holster>();
		perkManager = GetComponent<PerkManager>();
	}
	private void OnEnable()
	{
		inputAction.action.Enable();
	}
	private void OnDisable()
	{
		inputAction.action.Disable();
	}
	// Update is called once per frame
	void Update()
    {
        RaycastHit hit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionRange, layerMask))
		{
			Interactable tempInteractable;
			if (hit.collider.TryGetComponent(out tempInteractable))
			{
				if (interactable != tempInteractable)
				{
					if (hasInteractable)
						interactable.EndHover(this);
					interactable = tempInteractable;
				}
				interactable.StartHover(this);
				hasInteractable = true;
			}
			else
			{

				if (hasInteractable)
				{
					hasInteractable = false;
					interactable.EndHover(this);
				}
			}
		}
		else
		{

			if (hasInteractable)
			{
				hasInteractable = false;
				interactable.EndHover(this);
			}
		}
	}

    void Interact(InputAction.CallbackContext context)
	{
		if(!hasInteractable)
			return;
		interactable.Interact(this);
	}

	
}
