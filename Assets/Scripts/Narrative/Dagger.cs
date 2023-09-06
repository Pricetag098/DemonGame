using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    [SerializeField] private List<Transform> gates;
    [SerializeField] private List<Transform> mounds;
    [SerializeField] private List<Transform> graveInteractables;
    [SerializeField] private Transform falseDaggerInteractable;
    [SerializeField] private List<Transform> daggerModels;
    [SerializeField] private Transform interactable;
    private bool playerHasDagger = false;
    private bool playerHasFalseDagger = false;
    private bool playerHasShovel = false;
    [SerializeField] private List<AudioSource> audioSources;
    private Animator animator;
    private void Start() 
    {
    animator = GetComponent<Animator>();    
    }

    public void PickupDagger()
    {
        if(playerHasDagger)
        {
            if (playerHasFalseDagger)
            {
                foreach (Transform t in gates)
                {
                    t.gameObject.SetActive(false);
                }
                            animator.SetTrigger("PillarDown");
                daggerModels[1].gameObject.SetActive(true);
                interactable.gameObject.SetActive(false);
            }
            else
            {
                playerHasDagger = false;
                foreach (Transform t in gates)
                {
                    t.gameObject.SetActive(false);
                }
                            animator.SetTrigger("PillarDown");
                daggerModels[0].gameObject.SetActive(true);
            }
        }
        else
        {
            playerHasDagger = true;
            foreach (Transform t in gates)
            {
                t.gameObject.SetActive(true);
            }
            audioSources[0].Play();
            animator.SetTrigger("PillarUp");
            daggerModels[0].gameObject.SetActive(false);
        }
    }
    public void PickUpFalseDagger()
    {
        playerHasFalseDagger = true;
    }
    public void DigMoundOne()
    {
        mounds[0].gameObject.SetActive(true);
        graveInteractables.RemoveAt(0);
    }
    public void DigMoundTwo()
    {
        mounds[1].gameObject.SetActive(true);
        graveInteractables.RemoveAt(1);
        falseDaggerInteractable.gameObject.SetActive(true);
    }
    public void DigMoundThree()
    {
        mounds[2].gameObject.SetActive(true);
        graveInteractables.RemoveAt(2);
    }
    public void PickupShovel()
    {
        if (!playerHasShovel)
        {
            playerHasShovel = true;
            foreach (Transform t in graveInteractables)
            {
                t.gameObject.SetActive(true);
            }
        }
    }
    public void DropShovel()
    {
        playerHasShovel = false;
        foreach (Transform t in graveInteractables)
        {
            t.gameObject.SetActive(false);
        }
    }
}
