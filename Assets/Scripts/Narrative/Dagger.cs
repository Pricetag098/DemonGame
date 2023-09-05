using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    public List<Transform> gates;
    public List<Transform> mounds;
    public List<Transform> graveInteractables;
    public Transform falseDaggerInteractable;
    public List<Transform> daggerModels;
    public Transform interactable;
    private bool playerHasDagger = false;
    private bool playerHasFalseDagger = false;
    private bool playerHasShovel = false;

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
