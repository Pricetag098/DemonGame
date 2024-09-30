using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RitualWall : MonoBehaviour
{
    [SerializeField] float upTime;
    [SerializeField] float downTime;

    [SerializeField] Renderer front;
    [SerializeField] Renderer middle;
    [SerializeField] Renderer back;

    Material frontMaterial;
    Material middleMaterial;
    Material backMaterial;

    [SerializeField] float frontFlame;
    [SerializeField] float middleFlame;
    [SerializeField] float backFlame;

    [Header("Final Ritual Check box i know this is scuff")]

    [SerializeField] bool finalRitualDome = false;

    [SerializeField] Material finalFrontMaterial;
    [SerializeField] Material finalMiddleMaterial;
    [SerializeField] Material finalBackMaterial;

    void Start()
    {
        frontMaterial = front.sharedMaterial;
        middleMaterial = middle.sharedMaterial;
        backMaterial = back.sharedMaterial;

        if(finalRitualDome)
        {
            finalFrontMaterial.SetFloat("_WallRise", 3);
            finalMiddleMaterial.SetFloat("_WallRise", 3);
            finalBackMaterial.SetFloat("_WallRise", 3);
        }
    }

    public void Rise()
    {
        gameObject.SetActive(true);
        DG.Tweening.Sequence rise = DOTween.Sequence();
        rise.AppendCallback(() =>
        {
            DOTween.To(() => frontMaterial.GetFloat("_WallRise"), x => frontMaterial.SetFloat("_WallRise", x), frontFlame, upTime);
            DOTween.To(() => middleMaterial.GetFloat("_WallRise"), x => middleMaterial.SetFloat("_WallRise", x), middleFlame, upTime);
            DOTween.To(() => backMaterial.GetFloat("_WallRise"), x => backMaterial.SetFloat("_WallRise", x), backFlame, upTime);
        });
    }

    public void Fall()
    {
        DG.Tweening.Sequence fall = DOTween.Sequence();
        fall.AppendCallback(() =>
        {
            DOTween.To(() => frontMaterial.GetFloat("_WallRise"), x => frontMaterial.SetFloat("_WallRise", x), 3, downTime);
            DOTween.To(() => middleMaterial.GetFloat("_WallRise"), x => middleMaterial.SetFloat("_WallRise", x), 3, downTime);
            DOTween.To(() => backMaterial.GetFloat("_WallRise"), x => backMaterial.SetFloat("_WallRise", x), 3, downTime);
        });
        fall.AppendInterval(downTime);
        fall.AppendCallback(() => gameObject.SetActive(false));

    }
}
