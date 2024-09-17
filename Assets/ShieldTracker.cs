using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTracker : MonoBehaviour
{
    [SerializeField] int maxShield;

    [SerializeField] SoundPlayer player;

    [SerializeField] List<GameObject> shieldOnIcons;

    private int shields;

    public bool SpendShield()
    {
        if(shields <= 0)
        {
            return false;
        }
        else
        {
            shields--;
            shieldOnIcons[shields].SetActive(false);
            player.Play();
            return true;
        }
    }

    public bool CanRefill()
    {
        return shields < 3;
    }

    public void RefillShields()
    {
        foreach (GameObject item in shieldOnIcons)
        {
            item.SetActive(true);
        }
        shields = maxShield;
    }
}
