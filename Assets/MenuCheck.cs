using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCheck : MonoBehaviour
{

    PlayerSettings playerSettings;

    public GameObject SettingsManager;

    void Start()
    {
        playerSettings = SettingsManager.GetComponent<SettingsManager>().playerSettings;

        if(playerSettings.hasOpened)
        {
            Debug.Log("Off");
            this.gameObject.SetActive(false);
        }
    }

}
