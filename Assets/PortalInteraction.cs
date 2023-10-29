using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

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

	private void Awake()
	{
		Close();
		DOTween.Kill(this, true);
	}
	bool isOpen = true;	
	protected override bool CanBuy(Interactor interactor)
	{
		return !interactor.GetComponent<PlayerAbilityCaster>().caster.HasAbility(ability);
	}
	protected override void DoBuy(Interactor interactor)
	{
		interactor.caster.SetAbility(Instantiate(ability));
		Close();
	}

	public void Open()
    {
		if (isOpen)
			return; isOpen = true;
		Sequence open = DOTween.Sequence();
        open.Append(body.DOScale(Vector3.one * maxPortalSize, openTime)).SetEase(Ease.InSine);
        open.AppendCallback(() => armAnimator.SetTrigger("Out"));
		open.AppendCallback(() => armAnimator.ResetTrigger("In"));
		open.AppendCallback(() => openSound.Play());
		open.AppendCallback(() => idleSound.Play());
	}

   public void Close()
    {
		if(!isOpen)
			return; isOpen = false;
		Sequence close = DOTween.Sequence();
		close.AppendCallback(() => idleSound.Stop());
		close.AppendCallback(() => closeSound.Play());
		close.AppendCallback(() => armAnimator.SetTrigger("In"));
		close.AppendCallback(() => armAnimator.ResetTrigger("Out"));
		close.AppendInterval(animationTime);
		close.Append(body.DOScale(Vector3.one * minPortalSize, openTime)).SetEase(Ease.InSine);
		
        
	}
}
