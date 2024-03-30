using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitmarkerUi : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<PooledObject>().OnDespawn += () => {
            DOTween.Kill(this,true);
        };
    }

    public void Spawn(float showTime,float decayTime, float punchScale)
    {
        GetComponent<SoundPlayer>().Play();
        RectTransform rectTransform = GetComponent<RectTransform>();

        CanvasGroup group = GetComponent<CanvasGroup>();
        group.alpha = 1.0f;

        Sequence s = DOTween.Sequence(this);
        s.Append(rectTransform.DOPunchScale(Vector3.one * punchScale, showTime));
        s.Append(group.DOFade(0, decayTime));
    }



}
