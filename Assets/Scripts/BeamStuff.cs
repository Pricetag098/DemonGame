using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BeamStuff : MonoBehaviour
{
    [SerializeField] GameObject beamCast;

    VisualEffect vfx;

    private void Awake()
    {
        vfx = beamCast.GetComponentInChildren<VisualEffect>();
        vfx.Stop();
    }

    public void CastBeam()
    {
        beamCast.SetActive(true);
        vfx.Reinit();
    }

    public void WithdrawBeam()
    {
        vfx.Stop();
        beamCast.SetActive(false);
    }
}
