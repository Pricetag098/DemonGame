using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResurrectDisplay : MonoBehaviour
{
    PlayerDeath playerDeath;
    Image image;
    // Start is called before the first frame update
    void Awake()
    {
        playerDeath = FindObjectOfType<PlayerDeath>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.enabled = playerDeath.respawnsLeft > 0;
    }
}
