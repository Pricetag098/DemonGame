using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverTween : MonoBehaviour
{
    [SerializeField] Transform barrel;

    [SerializeField] Transform chamber;

    [SerializeField] float spinAmount = 45;

    [SerializeField] float spinTime = 0.25f;

    public void RotateBarrel()
    {
        barrel.DOLocalRotate(new Vector3(spinAmount, 0f, 0f), spinTime, RotateMode.LocalAxisAdd).SetRelative(true).SetEase(Ease.Linear);

        chamber.DOLocalRotate(new Vector3(spinAmount, 0f, 0f), spinTime, RotateMode.LocalAxisAdd).SetRelative(true).SetEase(Ease.Linear);
    }
}
