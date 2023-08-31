using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DemonCum 

{
    public class RoundDisplay : MonoBehaviour
    {
        SpawnerManager manager;
        public TextMeshProUGUI roundText;

        private void Awake()
        {
            roundText = GetComponentInChildren<TextMeshProUGUI>();
            manager = FindObjectOfType<SpawnerManager>();
        }

        private void Update()
        {
            roundText.text = manager.currentRound.ToString();
        }


    }
}
