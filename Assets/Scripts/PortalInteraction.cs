using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Sequence = DG.Tweening.Sequence;

public class PortalInteraction : ShopInteractable
{
    [SerializeField] Transform body;

    [SerializeField] float minPortalSize;

    [SerializeField] float maxPortalSize =1;

    [SerializeField] Animator armAnimator;
    [SerializeField] SoundPlayer openSound,idleSound,closeSound;

    [SerializeField] float openTime;
	[SerializeField] float animationTime;
    [SerializeField] Ability ability;

	[SerializeField] GlyphSpawning glyphSpawning;

    [SerializeField] PortalFrame portalFrame;

	AbilityNotification notification;

	bool hasAbility = false;

    private void Awake()
	{
		notification = FindObjectOfType<AbilityNotification>();
		Close(false);
		DOTween.Kill(this, true);
    }
    bool isOpen = true;	
	protected override bool CanBuy(Interactor interactor)
	{
		return !interactor.GetComponent<PlayerAbilityCaster>().caster.HasAbility(ability);
	}
	protected override void DoBuy(Interactor interactor)
	{
		interactor.caster.SetAbility(Instantiate(ability.upgradePath.Value.abilities[interactor.caster.upgradeNum]));
		notification.Notify(ability);
		Close(true);
	}

	public override void StartHover(Interactor interactor)
	{
		base.StartHover(interactor);
		interactor.display.DisplayMessage(true, buyMessage + " " + ability.abilityName + " ", "[Cost: " + GetCost(interactor).ToString() + "]");
	}

	public void Open()
    {
		if (!hasAbility)
		{
            if (isOpen)
                return; isOpen = true;
            Sequence open = DOTween.Sequence();
            portalFrame.StopEmissionBlink();
            glyphSpawning.DespawnAbility();
            open.Append(body.DOScale(Vector3.one * maxPortalSize, openTime)).SetEase(Ease.InSine);
            open.AppendCallback(() => armAnimator.SetTrigger("Out"));
            open.AppendCallback(() => armAnimator.ResetTrigger("In"));
            open.AppendCallback(() => openSound.Play());
            open.AppendCallback(() => idleSound.Play());
        }
    }

    public void Close(bool has)
    {
		if (!hasAbility)
		{
            if (!isOpen)
                return; isOpen = false;
            if (has) hasAbility = true;
            Sequence close = DOTween.Sequence();
            close.AppendCallback(() => idleSound.Stop());
            close.AppendCallback(() => closeSound.Play());
            close.AppendCallback(() => armAnimator.SetTrigger("In"));
            close.AppendCallback(() => armAnimator.ResetTrigger("Out"));
            close.AppendInterval(animationTime);
            close.Append(body.DOScale(Vector3.one * minPortalSize, openTime)).SetEase(Ease.InSine);
            close.AppendInterval(openTime);
            close.AppendCallback(() => glyphSpawning.SpawnAbility());
            close.AppendCallback(() => portalFrame.StartEmissionBlink());
        }
    }
}
