using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class HoverTween : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerExitHandler
{
    public SoundPlayer confirmSFX = null;
    public SoundPlayer hoverSFX = null;
    public float timeToTween;
    public float tweenScale;

    public bool confirm = false;

    public void TweenScale(float variable)
    {
        transform.DOScale(variable, timeToTween);
    }

    public void TweenSelect()
    {
        transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), timeToTween).OnComplete(() => { transform.DOScale(1, 0); });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverSFX.Play();
        transform.DOKill();
        TweenScale(tweenScale);    
    }

    public void OnSelect(BaseEventData eventData)
    {

        confirmSFX.Play();
        transform.DOKill();
        TweenSelect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        TweenScale(1);
    }
}
