using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BloodVolleyStuff : MonoBehaviour
{
    [SerializeField] GameObject bloodVolley;

    VisualEffect vfx;

    private void Awake()
    {
        vfx = bloodVolley.GetComponentInChildren<VisualEffect>();
        vfx.Stop();
    }

    public void CastVolley()
    {
        bloodVolley.SetActive(true);
        vfx.Reinit();
    }

    public void WithdrawVolley()
    {
        vfx.Stop();
        bloodVolley.SetActive(false);
    }
}
