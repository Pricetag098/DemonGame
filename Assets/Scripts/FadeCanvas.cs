using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;   

public class FadeCanvas : MonoBehaviour
{
    [SerializeField] public CanvasGroup canvasGroup;
    [SerializeField] public float duration = 1.5f;
    [SerializeField] public bool fadeIn;

    private void Awake()
    {
            canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        Fade();
    }

    public void Fade()
    {
        if (fadeIn)
        {
            canvasGroup.alpha = 1f;
            Sequence on = DOTween.Sequence();
            on.Append(canvasGroup.DOFade(0, duration));
            on.AppendCallback(() => canvasGroup.blocksRaycasts = false);

        }
        else
        {
            Sequence on = DOTween.Sequence();
            on.Append(canvasGroup.DOFade(1, duration));
            on.AppendCallback(() => canvasGroup.blocksRaycasts = false);
        }
    }
}
