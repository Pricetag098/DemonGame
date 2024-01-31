using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    PlayerStats playerStats;

    private void Update()
    {
        playerStats = GetComponent<PlayerStats>();

        if (GamePrefs.UnlockAllAbilities)
        {
            PortalInteraction[] abilBuys = FindObjectsOfType<PortalInteraction>(true);

            int points = playerStats.points;
            playerStats.points = 999999;

            for (int i = 0; i < abilBuys.Length; i++)
            {
                abilBuys[i].Interact(GetComponent<Interactor>());
            }
            playerStats.points = points;
        }
        Destroy(this);
    }
}
