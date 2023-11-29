using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BeamStuff : MonoBehaviour
{
    [SerializeField] GameObject beamCast;

    [SerializeField] float shrinkTime;

    Vector3 originalSize;

    private void Awake()
    {
        originalSize = beamCast.transform.localScale;
        beamCast.SetActive(false);
    }

    public void CastBeam()
    {
        Sequence cast = DOTween.Sequence();
        cast.AppendCallback(() => beamCast.SetActive(true));
        cast.Append(beamCast.transform.DOScale(originalSize, shrinkTime));
    }

    public void WithdrawBeam()
    {
        Sequence withdraw = DOTween.Sequence();
        withdraw.Append(beamCast.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), shrinkTime));
        withdraw.AppendCallback(() => beamCast.SetActive(false));
    }
}
