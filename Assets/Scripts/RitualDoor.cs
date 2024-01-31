using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualDoor : MonoBehaviour
{
    [SerializeField] GameObject book;
    [SerializeField] Animator doorAnimator;
    [SerializeField] RitualWall wall;
    [SerializeField] ArcaneLock arcaneLock;
    [SerializeField] GameObject interactable;
    [SerializeField] GameObject hoverObj;
    [SerializeField] List<Light> torchLights;
    [SerializeField] float lightIntensity;
    [SerializeField] float lightTime;
    [SerializeField] List<GameObject> torchFlames;


    private int ritualsCompleted;

    private void Start()
    {
        foreach (Light obj in torchLights)
        {
            obj.intensity = 0;
        }
    }

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
        wall.Fall();
        interactable.SetActive(true);
        hoverObj.SetActive(false);
        foreach(GameObject obj in torchFlames)
        {
            obj.SetActive(true);
        }
        foreach (Light obj in torchLights)
        {
            DOTween.To(() => obj.intensity, x => obj.intensity = x, lightIntensity, lightTime);
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
