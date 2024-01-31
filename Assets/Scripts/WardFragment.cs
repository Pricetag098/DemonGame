using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardFragment : MonoBehaviour
{
    public Vector3 endLocation;
    Vector3 startLocation;

    public float moveTime;

    Material dissolve;

    private void Awake()
    {
        dissolve = GetComponent<Renderer>().material;

        startLocation = transform.localPosition;
    }

    public void Off()
    {
        DOTween.Kill(this);
        transform.DOLocalMove(endLocation, moveTime);
        DOTween.To(() => dissolve.GetFloat("_Dissolve_Amount"), x => dissolve.SetFloat("_Dissolve_Amount", x), 1, moveTime);
    }

    public void On()
    {
        DOTween.Kill(this);
        transform.DOLocalMove(startLocation, moveTime);
        DOTween.To(() => dissolve.GetFloat("_Dissolve_Amount"), x => dissolve.SetFloat("_Dissolve_Amount", x), -1, moveTime);
    }
}
