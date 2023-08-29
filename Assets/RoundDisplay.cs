using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DemonCum 

{
    public class RoundDisplay : MonoBehaviour
    {
        DemonSpawner spawner;
        public TextMeshProUGUI roundText;


        private void Awake()
        {
            roundText = GetComponentInChildren<TextMeshProUGUI>();
            spawner = FindObjectOfType<DemonSpawner>();
        }

        private void Update()
        {
            roundText.text = spawner.currentRound.ToString();
        }


    }
}
