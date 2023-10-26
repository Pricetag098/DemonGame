using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalInteraction : ShopInteractable
{
    [SerializeField] Transform body;

    [SerializeField] Vector3 minPortalSize;

    [SerializeField] Vector3 maxPortalSize;

    [SerializeField] Animator armAnimator;
    [SerializeField] SoundPlayer openSound,idleSound,closeSound;

    [SerializeField] float openTime;

    [SerializeField] Ability ability;
    


    public void Open()
    {
        Sequence open = DOTween.Sequence();
        open.Append(body.DOScale(Vector3.one, openTime)).SetEase(Ease.InSine);
        open.AppendCallback(() => armAnimator.SetTrigger("In"));
        open.AppendCallback(() => openSound.Play());
		open.AppendCallback(() => idleSound.Play());
	}

   public void Close()
    {
		Sequence close = DOTween.Sequence();
		close.AppendCallback(() => idleSound.Stop());
		close.AppendCallback(() => closeSound.Play());
		close.AppendCallback(() => armAnimator.SetTrigger("Out"));
		close.Append(body.DOScale(Vector3.one, openTime)).SetEase(Ease.InSine);
		
        
	}
}
