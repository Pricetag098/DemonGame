using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class PortalInteraction : ShopInteractable
{
    [SerializeField] Transform body;
    [SerializeField] Collider interactableCollider;

    [SerializeField] float minPortalSize;

    [SerializeField] float maxPortalSize =1;

    [SerializeField] Animator armAnimator;
    [SerializeField] SoundPlayer openSound,idleSound,closeSound;

    [SerializeField] float openTime;
	[SerializeField] float animationTime;
    [SerializeField] Ability ability;

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
        interactableCollider.enabled = false;
        Close(true);
	}

	public override void StartHover(Interactor interactor)
	{
		base.StartHover(interactor);
		interactor.display.DisplayMessage(true, buyMessage + " " + ability.abilityName + " ", "[cost: " + GetCost(interactor).ToString() + "]");
	}

	public void Open()
    {
		if (!hasAbility)
		{
            if (isOpen)
                return; isOpen = true;
            DOTween.Kill(this, true);
            Sequence open = DOTween.Sequence();
            portalFrame.StopEmissionBlink();
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
            DOTween.Kill(this, true);
            Sequence close = DOTween.Sequence();
            close.AppendCallback(() => idleSound.Stop());
            close.AppendCallback(() => closeSound.Play());
            close.AppendCallback(() => armAnimator.SetTrigger("In"));
            close.AppendCallback(() => armAnimator.ResetTrigger("Out"));
            close.AppendInterval(animationTime);
            close.Append(body.DOScale(Vector3.one * minPortalSize, openTime)).SetEase(Ease.InSine);
            close.AppendInterval(openTime);
            close.AppendCallback(() => portalFrame.StartEmissionBlink());
        }
    }
}
