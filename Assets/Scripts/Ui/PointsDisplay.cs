using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PointsDisplay : MonoBehaviour
{
    PlayerStats playerStats;
    TextMeshProUGUI textMeshProUGUI;
    // Start is called before the first frame update
    void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textMeshProUGUI.text = playerStats.points.ToString();
    }
}
