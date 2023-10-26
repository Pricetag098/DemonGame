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
        public float colourChangeDuration;

        private void Awake()
        {
            roundText = GetComponentInChildren<TextMeshProUGUI>();
            manager = FindObjectOfType<SpawnerManager>();
        }

        private void Update()
        {
            roundText.text = manager.currentRound.ToString();
            
        }
        
        public void ColourChange()
        {
            roundText.DOColor(Color.white, colourChangeDuration).SetEase(Ease.InOutSine).OnComplete(() => 
            {
                roundText.DOColor(new Color(154, 44, 44), colourChangeDuration).SetEase(Ease.InOutSine);
            });

        }


    }
}
