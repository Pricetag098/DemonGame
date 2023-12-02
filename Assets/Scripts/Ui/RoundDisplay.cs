using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace DemonInfo 

{
    public class RoundDisplay : MonoBehaviour
    {
        SpawnerManager manager;
        public TextMeshProUGUI roundText;
        public CanvasGroup roundDisplayCanvasGroup;
        public float flashDuration;
        public float colourChangeDuration;

        public Color darkerRed = new Color(0.6f, 0.17f, 0.17f);

        private void Awake()
        {
            roundText = GetComponentInChildren<TextMeshProUGUI>();
            manager = FindObjectOfType<SpawnerManager>();
        }
        
        public void ColourFlash()
        {
            roundText.DOColor(Color.white, flashDuration).SetLoops(5, LoopType.Yoyo).OnComplete(() => 
            {
                roundDisplayCanvasGroup.DOFade(0, colourChangeDuration);
                ColourChangeWhite(); 
            });
        }

        public void ColourChangeWhite()
        {
            roundText.DOColor(Color.white, colourChangeDuration).OnComplete(() => 
            {
                ColourChangeRed();
                roundText.text = SpawnerManager.currentRound.ToString();
            });
        }

        public void ColourChangeRed() 
        {
            roundDisplayCanvasGroup.DOFade(1, colourChangeDuration);
            roundText.DOColor(darkerRed, colourChangeDuration).SetEase(Ease.InOutSine);
        }


    }
}
