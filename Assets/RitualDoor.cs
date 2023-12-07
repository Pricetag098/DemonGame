using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualDoor : MonoBehaviour
{
    [SerializeField] GameObject book;
    [SerializeField] Animator doorAnimator;
    [SerializeField] GameObject wall;
    [SerializeField] ArcaneLock arcaneLock;
    [SerializeField] GameObject interactable;
    [SerializeField] GameObject hoverObj;
    [SerializeField] List<GameObject> torchParts;

    private int ritualsCompleted;

    public void AddRitual()
    {
        ritualsCompleted++;
        if (ritualsCompleted == 3)
        {
            Unlock();
        }
    }

    [ContextMenu("Unlock")]
    void Unlock()
    {
        wall.SetActive(false);
        interactable.SetActive(true);
        hoverObj.SetActive(false);
        foreach(GameObject obj in torchParts)
        {
            obj.SetActive(true);
        }
    }

    [ContextMenu("Open")]
    public void Open()
    {
        book.SetActive(true);
        doorAnimator.SetTrigger("Open");
        arcaneLock.DisolveLock();
    }
}
