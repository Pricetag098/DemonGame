using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using DG.Tweening;
public class GunUpPickup : Interactable
{
    [SerializeField] Transform gunHolder;
    [SerializeField] Animator upgradeAnimator;
    [SerializeField] GameObject fakeGun;
    [SerializeField] float delay;
    public string pickUpMessage;

    //GameObject newGun;
    Gun newGun;
    GameObject upgrader;
    int heldIndex;
    GameObject worldGunOld;


    public bool finished = false;

    public override void StartHover(Interactor interactor)
    {
        base.StartHover(interactor);
        if (finished)
        {
            interactor.display.DisplayMessage(true, pickUpMessage + " " + newGun.gunName + " ", null);
        }
    }

    public override void Interact(Interactor interactor)
    {
        if (finished)
        {
            finished = false;
            upgrader.SetActive(true);
            Destroy(worldGunOld);
            GameObject gun = Instantiate(newGun.gameObject, interactor.holster.transform);
            Gun g = gun.GetComponent<Gun>();
            interactor.holster.HeldGun = g;
            g.gameObject.SetActive(false);
        }
    }

    public void Infused(Interactor player, Interactable upgradeInteractable, GameObject nextGun, Gun oldGun)
    {
        upgrader = upgradeInteractable.gameObject;
        upgrader.SetActive(false);

        GameObject gunModel = oldGun.worldModel;
        worldGunOld = Instantiate(gunModel, gunHolder);
        WorldGun worldGun = worldGunOld.GetComponent<WorldGun>();
        worldGun.ChangeMat(oldGun.tier);

        upgradeAnimator.SetTrigger("Upgrade");

        GameObject gun = Instantiate(fakeGun, player.holster.transform);
        Gun fake = gun.GetComponent<Gun>();
        player.holster.SetUpGun(fake);
        //heldIndex = player.holster.heldGunIndex;
        player.holster.ReplaceGun(fake);

        //newGun = nextGun;
        newGun = nextGun.GetComponent<Gun>();

    }

    public void SpawnNewGun()
    {
        worldGunOld.GetComponent<WorldGun>().ChangeMat(newGun.tier);
    }

    public void EnableGunPickup()
    {
        finished = true;
    }
}
