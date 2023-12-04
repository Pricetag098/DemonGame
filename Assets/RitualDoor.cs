using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualDoor : MonoBehaviour
{
    [SerializeField] GameObject book;
    [SerializeField] Animator doorAnimator;
    [SerializeField] GameObject wall;
    [SerializeField] ArcaneLock arcaneLock;

    private int ritualsCompleted;

    public void AddRitual()
    {
        ritualsCompleted++;
        if (ritualsCompleted == 3)
        {
            Open();
        }
    }

    [ContextMenu("Open")]
    public void Open()
    {
        book.SetActive(true);
        wall.SetActive(false);
        doorAnimator.SetTrigger("Open");
        arcaneLock.DisolveLock();
    }
}
