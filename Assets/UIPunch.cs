using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPunch : MonoBehaviour
{
    [SerializeField] GameObject objectToPunch;

    [SerializeField] Vector3 punchScale;
    [SerializeField] int punchVibrado = 10;
    [SerializeField] float punchElasticity = 1;
    [SerializeField] float punchTime;

    public void Punch()
    {
        DOTween.Kill(this);
        objectToPunch.transform.DOPunchScale(punchScale, punchTime, punchVibrado, punchElasticity);
    }
}
