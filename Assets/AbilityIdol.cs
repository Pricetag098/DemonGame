using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class AbilityIdol : Interactable
{
    [SerializeField] Slider meter;

    public float maxBlood;
    public float firstAbilityAmount;
    public float blood;

    [SerializeField] GameObject upgrader;

    [SerializeField] Ability firstAbility;
    [SerializeField] Ability secondAbility;

    [SerializeField] public string interactMessage = "To buy ";
    [SerializeField] Optional<SoundPlayer> interactSound;

    private AbilityCaster abilityCaster;

    AbilityNotification notification;

    bool hasFirstAbility;

    private void Awake()
    {
        notification = FindObjectOfType<AbilityNotification>();

        UpdateMeter();
    }

    public override void Interact(Interactor interactor)
    {
        abilityCaster = interactor.GetComponent<AbilityCaster>();

        blood += abilityCaster.blood;
        abilityCaster.RemoveBlood(abilityCaster.blood);

        UpdateMeter();

        if (blood > maxBlood * firstAbilityAmount && !hasFirstAbility)
        {
            interactor.caster.SetAbility(Instantiate(firstAbility.upgradePath.Value.abilities[interactor.caster.upgradeNum]));
            notification.Notify(firstAbility);
            hasFirstAbility = true;
        }
        else if(blood >= maxBlood)
        {
            interactor.caster.SetAbility(Instantiate(secondAbility.upgradePath.Value.abilities[interactor.caster.upgradeNum]));
            notification.Notify(secondAbility);
            DeActivate();
        }
    }

    private void UpdateMeter()
    {
        meter.value = blood / maxBlood;
    }

    private void DeActivate()
    {
        upgrader.SetActive(true);
        gameObject.SetActive(false);
    }

    public override void EndHover(Interactor interactor)
    {
        base.EndHover(interactor);
        interactor.display.HideText();
    }
    public override void StartHover(Interactor interactor)
    {
        base.StartHover(interactor);
        interactor.display.DisplayMessage(true, interactMessage, null);
    }

    [ContextMenu("Interact")]
    public void TestBuy()
    {
        Interact(FindObjectOfType<Interactor>());
    }
}
