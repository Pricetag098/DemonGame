using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScreen : MonoBehaviour
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

    PlayerStats stats;

    [SerializeField] TextMeshProUGUI pointsText, killsText, headShotsText, bloodGainText, deathsText,roundText;

    private void Awake()
    {
        endTitleCanvas = endTitle.GetComponent<CanvasGroup>();
        endStatsCanvas = endStats.GetComponent<CanvasGroup>();
        overlayCanvas = overlay.GetComponent<CanvasGroup>();
        stats = FindObjectOfType<PlayerStats>();
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
    public void Open()
    {
        Time.timeScale = 0;
        roundText.text = "you survived " + SpawnerManager.currentRound + " rounds";
        pointsText.text = stats.pointsGained.ToString();
        killsText.text = stats.kills.ToString();
        headShotsText.text = stats.headshotKills.ToString();
        bloodGainText.text = stats.GetComponent<PlayerAbilityCaster>().bloodSpent.ToString();
        deathsText.text = stats.deaths.ToString();


        Sequence on = DOTween.Sequence();
        on.SetUpdate(true);
        on.Append(overlayCanvas.DOFade(1, titleFadeTime + titleMoveTime + titleOnScreenTime + statsFadeTime).SetUpdate(true));
        on.Append(endTitleCanvas.DOFade(1, titleFadeTime));
        on.AppendInterval(titleOnScreenTime);
        on.AppendCallback(() => titleRectTransform.DOLocalMove(titleEndPos, titleMoveTime).SetUpdate(true));
        on.AppendInterval(titleMoveTime);
        on.Append(endStatsCanvas.DOFade(1, statsFadeTime));
        on.AppendCallback(() => 
        { 
            endStatsCanvas.interactable = true; 
            endStatsCanvas.blocksRaycasts = true;
            endTitleCanvas.interactable = true;
            endTitleCanvas.blocksRaycasts = true;
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
