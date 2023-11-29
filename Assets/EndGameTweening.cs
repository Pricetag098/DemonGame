using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTweening : MonoBehaviour
{
    [SerializeField] GameObject endTitle;
    [SerializeField] GameObject endStats;

    [SerializeField] float titleFadeTime;
    [SerializeField] float titleOnScreenTime;
    [SerializeField] float titleMoveTime;
    [SerializeField] float statsFadeTime;

    [SerializeField] Vector3 titleEndPos;
    Vector3 titleOrigin;

    RectTransform titleRectTransform;
    RectTransform scoreRectTransform;

    CanvasGroup endTitleCanvas;
    CanvasGroup endStatsCanvas;

    private void Awake()
    {
        endTitleCanvas = endTitle.GetComponent<CanvasGroup>();
        endStatsCanvas = endStats.GetComponent<CanvasGroup>();

        titleRectTransform = endTitle.GetComponent<RectTransform>();
        scoreRectTransform = endStats.GetComponent<RectTransform>();

        titleOrigin = titleRectTransform.localPosition;
    }

    private void Start()
    {
        titleRectTransform.localPosition = titleOrigin;

        endTitleCanvas.alpha = 0;
        endStatsCanvas.alpha = 0;
    }

    [ContextMenu("Tween")]
    public void TweenUI()
    {
        Sequence on = DOTween.Sequence();
        on.Append(endTitleCanvas.DOFade(1, titleFadeTime));
        on.AppendInterval(titleOnScreenTime);
        on.AppendCallback(() =>
        {
            titleRectTransform.DOLocalMove(titleEndPos, titleMoveTime);
        });
        on.AppendInterval(titleMoveTime);
        on.Append(endStatsCanvas.DOFade(1, statsFadeTime));
    }
}
