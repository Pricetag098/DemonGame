using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBox : MonoBehaviour
{
    public int currentSouls;
    [SerializeField] int maxSouls;

    public bool active = false;

    [SerializeField] PerkBuy perk;

    public void AddSoul()
    {
        if(currentSouls < maxSouls)
        {
            currentSouls++;

            if(currentSouls >= maxSouls)
            {
                OnComplete();
            }
        }
    }

    private void OnComplete()
    {
        perk.Upgrade(GameManager.instance.Player.GetComponent<PerkManager>());
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(active == true)
        {
            if (other.TryGetComponent(out HitBox hit))
            {
                if (hit.health.TryGetComponent(out LesserDemon d))
                {
                    d.SoulBox = this;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(active == true)
        {
            if (other.TryGetComponent(out HitBox hit))
            {
                if (hit.health.TryGetComponent(out LesserDemon d))
                {
                    d.SoulBox = null;
                }
            }
        }
    }
}
