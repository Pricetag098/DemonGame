using DG.Tweening;
using System.Threading;
using UnityEngine;
using Material = UnityEngine.Material;

public class ResurrectionBuy : ShopInteractable
{
    PlayerDeath playerDeath;
    int buys = 0;
    public int buyLimit = 3;
    public string usedUpMessage;
    public string alreadyOwnsMessage;
    public float maxEmission;
    public float minEmission;
    public float blinkTime;

    public GameObject fountainHeads;
    public GameObject fountain;

    int setCost;

    Material emissiveMat;
    Material emissiveHeadMat;
    public Color originalColour;
    public float emissionIntesity = 0;
    public float emissionHeadIntesity = 0;
    float headTargetEmission;
    float baseTargetEmission;

    private bool turnOffHeadEmission = false;
    private bool turnOffBaseEmission = false;
    private bool noHead = false;
    private bool noBase = false;
    private bool maxEmissionHead = false;

    private void Awake()
    {
        playerDeath = FindObjectOfType<PlayerDeath>();
        emissiveMat = fountain.GetComponent<Renderer>().material;
        emissiveHeadMat = fountainHeads.GetComponent<Renderer>().material;
        originalColour = emissiveMat.GetColor("_EmissionColour");
        setCost = Cost;
        Cost = 0;
    }

    private void Start()
    {
        Blink(ref baseTargetEmission, minEmission, false);
        Blink(ref headTargetEmission, minEmission, true);
    }

    protected override bool CanBuy(Interactor interactor)
    {
        return playerDeath.respawnsLeft == 0 && buys < buyLimit;
    }

    protected override void DoBuy(Interactor interactor)
    {
        if (buys < buyLimit - 1)
        {
            turnOffBaseEmission = true;
            maxEmissionHead = true;
            Cost = setCost;
        }
        else
        {
            maxEmissionHead = false;
            turnOffHeadEmission = true;
            turnOffBaseEmission = true;
        }
        base.DoBuy(interactor);
        playerDeath.respawnsLeft++;
        buys++;
    }

    public override void StartHover(Interactor interactor)
    {
        base.StartHover(interactor);

        if (buys >= buyLimit)
        {
            interactor.display.DisplayMessage(false, usedUpMessage);
        }
        else if (playerDeath.respawnsLeft > 0)
        {
            interactor.display.DisplayMessage(false, alreadyOwnsMessage);
        }
        else
        {
            interactor.display.DisplayMessage(true, buyMessage + ": " + Cost);
        }
    }

    private void Update()
    {
        if (!noBase)
        {
            FlashEmission(ref emissionIntesity, ref baseTargetEmission, emissiveMat, turnOffBaseEmission, false);
        }
        if (!noHead)
        {
            FlashEmission(ref emissionHeadIntesity, ref headTargetEmission, emissiveHeadMat, turnOffHeadEmission, true);
        }
    }

    private void FlashEmission(ref float emissionIntens, ref float targetEmission, Material emisMat, bool turnOff, bool head)
    {
        Color newEmissionColor = originalColour * emissionIntens;

        emisMat.SetColor("_EmissionColour", newEmissionColor);

        if (emissionIntens == targetEmission && targetEmission == maxEmission)
        {
            if (head)
            {
                if (maxEmissionHead) { noHead = true; return; }
            }

            if (head) Blink(ref targetEmission, minEmission, head);
            else Blink(ref targetEmission, minEmission, head);
        }
        else if (emissionIntens == targetEmission && targetEmission == minEmission)
        {
            if (turnOff)
            {
                if (head) noHead = true;
                else noBase = true;
            }
            else 
            {
                if (head) Blink(ref targetEmission, maxEmission, head);
                else Blink(ref targetEmission, maxEmission, head);
            }
        }
    }

    public void Blink(ref float targetEmission, float targetAmount, bool head)
    {
        if (head)
        {
            DOTween.To(() => emissionHeadIntesity, x => emissionHeadIntesity = x, targetAmount, blinkTime);
        }
        else
        {
            DOTween.To(() => emissionIntesity, x => emissionIntesity = x, targetAmount, blinkTime);
        }
        targetEmission = targetAmount;
    }

    public void EnableEmission()
    {
        if (buys < buyLimit)
        {
            maxEmissionHead = false;
            Sequence turnOn = DOTween.Sequence();
            turnOn.AppendCallback(() => { turnOffBaseEmission = false; noBase = false; });
            turnOn.AppendInterval(blinkTime);
            turnOn.AppendCallback(() => { turnOffHeadEmission = false; noHead = false; });
        }
    }
}
