using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CasterDisplay : MonoBehaviour
{
    [SerializeField] Image[] icons;
    [SerializeField] Slider bloodMeter;
    [SerializeField] AbilityCaster caster;
    // Start is called before the first frame update
    void Awake()
    {
        caster = FindObjectOfType<AbilityCaster>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < icons.Length; i++)
		{
            icons[i].sprite = caster.abilities[i].icon;
		}
        bloodMeter.value = caster.blood / caster.maxBlood;
    }
}
