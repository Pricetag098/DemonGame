using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CasterDisplay : MonoBehaviour
{
    [SerializeField] Image[] icons;
    [SerializeField] Image bloodMeter;
    [SerializeField] AbilityCaster caster;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < icons.Length; i++)
		{
            icons[i].sprite = caster.abilities[i].icon;
		}
        bloodMeter.fillAmount = caster.blood / caster.maxBlood;
    }
}
