using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResurrectDisplay : MonoBehaviour
{
    public PlayerDeath playerDeath;
    public Image image;
    // Start is called before the first frame update
    void Start()
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
