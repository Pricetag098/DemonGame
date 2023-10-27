using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class HoverTween : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerExitHandler
{
    public float timeToTween;
    public float tweenScale;

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
        transform.DOKill();
        TweenScale(tweenScale);    
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.DOKill();
        TweenSelect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        TweenScale(1);
    }
}
