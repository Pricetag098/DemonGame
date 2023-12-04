using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameTweening : MonoBehaviour
{
    [SerializeField] GameObject endTitle;
    [SerializeField] GameObject endStats;
    [SerializeField] GameObject overlay;

    [SerializeField] float titleFadeTime;
    [SerializeField] float titleOnScreenTime;
    [SerializeField] float titleMoveTime;
    [SerializeField] float statsFadeTime;

    [SerializeField] Vector3 titleEndPos;
    Vector3 titleOrigin;

    RectTransform titleRectTransform;

    CanvasGroup endTitleCanvas;
    CanvasGroup endStatsCanvas;
    CanvasGroup overlayCanvas;

    private void Awake()
    {
        endTitleCanvas = endTitle.GetComponent<CanvasGroup>();
        endStatsCanvas = endStats.GetComponent<CanvasGroup>();
        overlayCanvas = overlay.GetComponent<CanvasGroup>();

        titleRectTransform = endTitle.GetComponent<RectTransform>();

        titleOrigin = titleRectTransform.localPosition;
    }

    private void Start()
    {
        titleRectTransform.localPosition = titleOrigin;

        endTitleCanvas.alpha = 0;
        endStatsCanvas.alpha = 0;
        overlayCanvas.alpha = 0;
    }

    [ContextMenu("Tween")]
    public void TweenUI()
    {
        Sequence on = DOTween.Sequence();
        overlayCanvas.DOFade(1, titleFadeTime + titleMoveTime + titleOnScreenTime + statsFadeTime);
        on.Append(endTitleCanvas.DOFade(1, titleFadeTime));
        on.AppendInterval(titleOnScreenTime);
        on.AppendCallback(() => titleRectTransform.DOLocalMove(titleEndPos, titleMoveTime));
        on.AppendInterval(titleMoveTime);
        on.Append(endStatsCanvas.DOFade(1, statsFadeTime));
        on.AppendCallback(() => 
        { 
            endStatsCanvas.interactable = true; 
            endStatsCanvas.blocksRaycasts = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        });
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
