using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResurrectDisplay : MonoBehaviour
{
    public PlayerDeath playerDeath;
    public GameObject rebirthStoneIcon;
    // Start is called before the first frame update
    void Start()
    {
        playerDeath = FindObjectOfType<PlayerDeath>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerDeath.respawnsLeft > 0)
        {
            rebirthStoneIcon.SetActive(true);
        }
        else
        {
            rebirthStoneIcon.SetActive(false);
        }
    }
}
