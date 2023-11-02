using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    [SerializeField] private List<GameObject> gates;
    [SerializeField] private List<GameObject> graveInteractables;
    [SerializeField] private List<GameObject> graveInteractablesToEnable;
    [SerializeField] private GameObject falseDaggerInteractable;
    [SerializeField] private List<GameObject> daggerModels;
    [SerializeField] private GameObject interactable;
    [SerializeField] private bool playerHasDagger = false;
    [SerializeField] private bool playerHasFalseDagger = false;
    [SerializeField] private bool playerHasShovel = false;
    [SerializeField] private List<AudioSource> audioSources;
    private Animator animator;

    private void Awake() 
    {
    animator = GetComponent<Animator>();
    foreach (GameObject g in graveInteractables) 
        {
            graveInteractablesToEnable.Add(g);
        }
    }

    public void PickupDagger()
    {
        if(playerHasDagger)
        {
            if (playerHasFalseDagger)
            {
                foreach (GameObject g in gates)
                {
                    g.SetActive(false);
                }
                            animator.SetTrigger("PillarStop");
                audioSources[2].Play();
                daggerModels[1].gameObject.SetActive(true);
                interactable.SetActive(false);
            }
            else
            {
                playerHasDagger = false;
                foreach (GameObject g in gates)
                {
                    g.SetActive(false);
                }
                            animator.SetTrigger("PillarDown");
                audioSources[2].Play();
                daggerModels[0].gameObject.SetActive(true);
            }
        }
        else
        {
            playerHasDagger = true;
            foreach (GameObject g in gates)
            {
                g.SetActive(true);
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
    public void DigMound(GameObject mound)
    {
        graveInteractablesToEnable.RemoveAt(graveInteractables.IndexOf(mound));
        audioSources[1].PlayDelayed(1);
        if(graveInteractables.IndexOf(mound) == 1)
        {
            falseDaggerInteractable.SetActive(true);
        }
    }
    public void PickupShovel()
    {
        if (!playerHasShovel)
        {
            playerHasShovel = true;
            foreach (GameObject g in graveInteractablesToEnable)
            {
                g.SetActive(true);
            }
        }
    }
    public void DropShovel()
    {
        playerHasShovel = false;
        foreach (GameObject g in graveInteractablesToEnable)
        {
            g.SetActive(false);
        }
    }
}
