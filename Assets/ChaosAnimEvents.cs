using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosAnimEvents : MonoBehaviour
{
    [System.Serializable]
    public class ChaosAnims
    {
        public Vector3 position; //pos of cast during animation

        public Transform parent; //parent of cast during anim

        public Vector3 maxSize; //max size during anim

        public Vector3 rotation; //rot during anim

        public float openTime;

        public float closeTime;
    }

    [SerializeField] ChaosAnims[] animations;

    [SerializeField] GameObject circle;

    public void Cast(int animNum)
    {
        DOTween.Kill(this);

        circle.SetActive(true);

        ChaosAnims anim = animations[animNum];

        circle.transform.parent = anim.parent;

        circle.transform.localPosition = anim.position;

        circle.transform.localRotation = Quaternion.Euler(anim.rotation);

        circle.transform.DOScale(anim.maxSize, anim.openTime);
    }

    public void Close(int animNum)
    {
        ChaosAnims anim = animations[animNum];
        DOTween.Kill(this);
        Sequence close = DOTween.Sequence();
        close.Append(circle.transform.DOScale(new Vector3(0, 0, 0), anim.closeTime));
        close.AppendCallback(() => circle.SetActive(true));
    }
}
