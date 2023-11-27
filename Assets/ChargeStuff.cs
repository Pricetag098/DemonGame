using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ChargeStuff : MonoBehaviour
{
    public VisualEffect effect;

    private void Start()
    {
        effect.Stop();
    }

    public void ChargeStage1()
    {
        effect.SendEvent("ChargeStage1");
    }

    public void ChargeStage2()
    {
        effect.SendEvent("ChargeStage2");
    }

    public void ChargeStage3()
    {
        effect.SendEvent("ChargeStage3");
    }

    public void StopEffect()
    {
        effect.Stop();
    }
}
