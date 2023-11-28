using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BloodVolleyStuff : MonoBehaviour
{
    [SerializeField] GameObject bloodVolley;

    [SerializeField] float shrinkTime;

    Vector3 originalSize;

    private void Awake()
    {
        originalSize = bloodVolley.transform.localScale;
        bloodVolley.SetActive(false);
    }

    public void CastVolley()
    {
        Sequence cast = DOTween.Sequence();
        cast.AppendCallback(() => bloodVolley.SetActive(true));
        cast.Append(bloodVolley.transform.DOScale(originalSize, shrinkTime));
    }

    public void WithdrawVolley()
    {
        Sequence withdraw = DOTween.Sequence();
        withdraw.Append(bloodVolley.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), shrinkTime));
        withdraw.AppendCallback(() => bloodVolley.SetActive(false));
    }
}
