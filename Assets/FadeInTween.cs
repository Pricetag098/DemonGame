using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FadeInTween : MonoBehaviour
{
    DirectionTweens directionTweens;
    private Vector2 startPosition;
    private Vector2 endPosition = new Vector2 (0,0);

    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;
    public float duration;

    private Image image = null;

    public bool single;
    public bool moveIn = false;
    public Ease easeType;

    public TweenDirection easeDirection;

    private void Awake()
    {
        directionTweens = new DirectionTweens();
        directionTweens.ChooseTweenDirection(easeDirection);
        startPosition = directionTweens.startPosition;

        if (single)
        {
            image = GetComponent<Image>();
        }
    }
    public void TweenIn()
    {
        if (moveIn)
        {
            rectTransform.transform.localPosition = startPosition;
            rectTransform.DOAnchorPos(endPosition, duration, false).SetEase(easeType);
        }

        if (single)
        {
            image.DOFade(1, duration);
        }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.DOFade(1, duration);
        }
    }

    /*public void TweenOut()
    {
        if (moveIn)
        {
            rectTransform.transform.localPosition = new Vector3(0f, 0f, 0f);
            rectTransform.DOAnchorPos(new Vector2(0f, -1000f), duration, false).SetEase(easeType);
        }

        if (single)
        {
            image.DOFade(0, duration);
        }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.DOFade(0, duration);
        }
    }*/
}
