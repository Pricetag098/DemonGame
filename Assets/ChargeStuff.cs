using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class ChargeStuff : MonoBehaviour
{
    public VisualEffect handEffects;

    public VisualEffect speedEffects;

    public Volume speedScreen;

    private void Start()
    {
        handEffects.Stop();
        speedEffects.Stop();

        speedScreen.weight = 0;
    }

    public void ChargeStage1()
    {
        handEffects.SendEvent("ChargeStage1");
    }

    public void ChargeStage2()
    {
        handEffects.SendEvent("ChargeStage2");
    }

    public void ChargeStage3()
    {
        handEffects.SendEvent("ChargeStage3");
    }

    public void StopHand()
    {
        handEffects.Stop();
    }

    public void StartSpeed()
    {
        speedEffects.gameObject.SetActive(true);
        speedEffects.Play();
        DOTween.To(() => speedScreen.weight, x => speedScreen.weight = x, 1f, 0.4f);
    }

    public void StopSpeed()
    {
        speedEffects.gameObject.SetActive(false);
        speedEffects.Stop();
        DOTween.To(() => speedScreen.weight, x => speedScreen.weight = x, 0f, 0.4f);
    }
}
