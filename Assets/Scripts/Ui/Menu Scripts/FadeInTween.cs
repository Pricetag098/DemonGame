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

    public float startValue;

    public CanvasGroup canvasGroup = null;
    public RectTransform rectTransform = null;

    [Header("Durations")]
    public float fadeDuration;
    public float moveDuration;

    private Image image = null;

    public Ease easeType;

    [Header("Bools")]
    public bool single;
    public bool moveIn = false;

    public TweenDirection easeDirection;

    private void Awake()
    {
        directionTweens = new DirectionTweens();

        directionTweens.ChooseTweenDirection(easeDirection, startValue);
        startPosition = directionTweens.startPosition;

        if (single) { image = GetComponent<Image>(); rectTransform = GetComponent<RectTransform>(); }
    }
    public void TweenIn()
    {
        if (moveIn)
        {
            rectTransform.transform.localPosition = startPosition;
            rectTransform.DOAnchorPos(endPosition, moveDuration, false).SetEase(easeType).SetAutoKill(false);
        }

        if (single) { image.DOFade(1, fadeDuration); }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.DOFade(1, fadeDuration).SetAutoKill(false);
        }
    }
}
