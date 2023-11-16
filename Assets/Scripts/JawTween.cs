using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JawTween : MonoBehaviour
{
    [SerializeField] Transform jaw;

    [SerializeField] float biteOpenTime;

    [SerializeField] float biteCloseTime;

    [SerializeField] float biteAmount;

    [SerializeField] float jawOpenAmount;

    [SerializeField] float jawOpenTime;

    [SerializeField] float jawCloseTime;

    Vector3 originalJawPos;

    private void Start()
    {
        jaw.position = originalJawPos;
    }

    public void OpenJaw()
    {
        jaw.DOMoveY(-jawOpenAmount, jawOpenTime);
    }

    public void CloseJaw()
    {
        jaw.DOMoveY(jawOpenAmount, jawCloseTime);
    }

    public void Bite()
    {
        Sequence bite = DOTween.Sequence();

        bite.Append(jaw.DOMoveY(-biteAmount, biteOpenTime));
        bite.AppendInterval(biteOpenTime);
        bite.Append(jaw.DOMoveY(biteAmount, biteCloseTime));

    }
}
