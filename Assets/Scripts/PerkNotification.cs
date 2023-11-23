using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PerkNotification : MonoBehaviour
{
    public TextMeshProUGUI perkIcon;
    public TextMeshProUGUI perkDescription;
    public float fadeTime;
    public float onScreenDuration;

    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Notify(Perk perk)
    {
        perkIcon.text = perk.symbolText;
        perkIcon.font = perk.perkFont;
        perkDescription.text = perk.description;
        Sequence fade = DOTween.Sequence();
        fade.AppendCallback(() => canvasGroup.DOFade(1, fadeTime));
        fade.AppendInterval(onScreenDuration);
        fade.AppendCallback(() => canvasGroup.DOFade(0, fadeTime));
    }
}
