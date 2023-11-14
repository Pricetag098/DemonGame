using DG.Tweening;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using UnityEditor;
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
	Material emissiveMat;
    Material emissiveHeadMat;
    Color originalColour;
	float emissionIntesity = 0;
	float emissionHeadIntesity = 0;
    [HideInInspector] public bool turnOff = false;
    [HideInInspector] public bool doNothing = false;
    [HideInInspector] public bool blink = true;
    bool endBlink = false;
    [HideInInspector] public bool blinkOn = false;
	float headTargetEmission;
	float baseTargetEmission;

    private void Awake()
	{
		playerDeath = FindObjectOfType<PlayerDeath>();
		emissiveMat = fountain.GetComponent<Renderer>().material;
		emissiveHeadMat = fountainHeads.GetComponent<Renderer>().material;
        originalColour = emissiveMat.GetColor("_EmissionColour");
		emissionIntesity = maxEmission;
		emissionHeadIntesity = maxEmission;
    }

	private void Start()
	{
		EmissionOff();
		FinishHeadBlink();
		blink = true;
        turnOff = true;
        blinkOn = true;
	}

	protected override bool CanBuy(Interactor interactor)
	{
		return playerDeath.respawnsLeft == 0 && buys < buyLimit;
	}

	protected override void DoBuy(Interactor interactor)
	{
		if (buys < buyLimit - 1)
		{
			EmissionOff();
            FinishHeadBlink();
            blink = true;
            blinkOn = true;
            turnOff = true;
        }
        else
		{
            EmissionOff();
            FinishHeadBlink();
            blink = true;
            blinkOn = false;
            endBlink = true;
            turnOff = true;
		}
        base.DoBuy(interactor);
		playerDeath.respawnsLeft++;
		buys++;
    }

    public override void StartHover(Interactor interactor)
    {
        base.StartHover(interactor);
        
		if(buys >= buyLimit)
		{
            interactor.display.DisplayMessage(false, usedUpMessage);
        }
		else if(playerDeath.respawnsLeft >0)
		{
			interactor.display.DisplayMessage(false, alreadyOwnsMessage);
		}
		else
		{
			interactor.display.DisplayMessage(true, buyMessage);
		}
    }

	private void Update()
	{
		if (!doNothing)
		{
			ChangeEmission(baseTargetEmission);
        }

		if (blink)
		{
            ChangeHeadEmissions(headTargetEmission);
        }
    }

	private void ChangeEmission(float targetEmission)
	{
        Color newEmissionColor = originalColour * emissionIntesity;

        emissiveMat.SetColor("_EmissionColour", newEmissionColor);

        if (emissionIntesity == targetEmission && targetEmission == minEmission && turnOff)
        {
            doNothing = true;
        }
        else if (emissionIntesity == targetEmission && targetEmission == minEmission)
        {
            EmissionOn();
        }
        else if (emissionIntesity == targetEmission && targetEmission == maxEmission)
        {
            EmissionOff();
        }
    }

    private void ChangeHeadEmissions(float targetEmission)
	{
        Color newEmissionColor = originalColour * emissionHeadIntesity;

        emissiveHeadMat.SetColor("_EmissionColour", newEmissionColor);

		if (emissionHeadIntesity == targetEmission && targetEmission == minEmission && endBlink)
        {
            blink = false;
        }
        else if (emissionHeadIntesity == targetEmission && targetEmission == maxEmission && blinkOn)
        {
            blink = false;
        }
        else if (emissionHeadIntesity == targetEmission && targetEmission == minEmission)
		{
			StartHeadBlink();
        }
        else if (emissionHeadIntesity == targetEmission && targetEmission == maxEmission)
		{
            FinishHeadBlink();
        }

    }

    public void StartHeadBlink()
    {
        DOTween.To(() => emissionHeadIntesity, x => emissionHeadIntesity = x, maxEmission, blinkTime);
		headTargetEmission = maxEmission;
    }

    public void FinishHeadBlink()
    {
        DOTween.To(() => emissionHeadIntesity, x => emissionHeadIntesity = x, minEmission, blinkTime);
        headTargetEmission = minEmission;
    }

    public void EmissionOn()
	{
        DOTween.To(() => emissionIntesity, x => emissionIntesity = x, maxEmission, blinkTime);
        baseTargetEmission = maxEmission;
    }

    public void EmissionOff()
    {
        DOTween.To(() => emissionIntesity, x => emissionIntesity = x, minEmission, blinkTime);
        baseTargetEmission = minEmission;
    }
}
