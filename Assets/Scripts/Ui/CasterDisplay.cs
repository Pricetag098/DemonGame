using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CasterDisplay : MonoBehaviour
{
    [SerializeField] Image[] icons;
    [SerializeField] Slider bloodMeter;
    [SerializeField] PlayerAbilityCaster caster;
    // Start is called before the first frame update
    void Awake()
    {
        caster = FindObjectOfType<PlayerAbilityCaster>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < icons.Length; i++)
		{
            icons[i].sprite = caster.caster.abilities[i].icon;
            icons[i].GetComponent<Outline>().enabled = i == caster.activeIndex;
		}
        bloodMeter.value = caster.caster.blood / caster.caster.maxBlood;
    }
}
