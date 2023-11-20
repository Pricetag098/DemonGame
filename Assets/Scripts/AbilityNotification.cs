using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class AbilityNotification : MonoBehaviour
{
    public TextMeshProUGUI abilityIcon;
    public TextMeshProUGUI abilityName;
    public float fadeTime;
    public float onScreenDuration;

    float timer = 0;

    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Notify(Ability ability)
    {
        abilityIcon.text = ability.symbolText;
        abilityName.text = ability.abilityName + " Unlocked";
        Sequence fade = DOTween.Sequence();
        fade.AppendCallback(() => canvasGroup.DOFade(1, fadeTime));
        fade.AppendInterval(onScreenDuration);
        fade.AppendCallback(() => canvasGroup.DOFade(0, fadeTime));
    }
}
