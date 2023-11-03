using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ActiveAbilityName : MonoBehaviour
{
    PlayerAbilityCaster playerAbilityCaster;
    AbilityCaster abilityCaster;


    [SerializeField] TMP_FontAsset[] fontList;
    [SerializeField] TMP_FontAsset currentFont;

    [SerializeField] TextMeshProUGUI abilityNameText;

    [SerializeField] string[] phrases;
    [SerializeField] int maxPhrases;

    [SerializeField] float lingerTime;

    void Awake()
    {
        abilityCaster = FindObjectOfType<AbilityCaster>();
        playerAbilityCaster = FindObjectOfType<PlayerAbilityCaster>();
    }

    /*public void OnSwitchAbility()
    {
        List<int> ints = new List<int>();
        Sequence sequence = DOTween.Sequence();

        currentFont = fontList[0];
        abilityNameText.font = currentFont;

        for (int i = 0; i < maxPhrases; i++)
        {
            i = Random.Range(0, phrases.Length);
            ints.Add(i);
        }

        foreach (int value in ints)
        {
            sequence.Append(abilityNameText.DOFade(1, lingerTime).OnComplete(() => { abilityNameText.text = phrases[value]; }));
        }
        
        ints.Clear();

        currentFont = fontList[1];
        abilityNameText.font = currentFont;

        abilityNameText.text = abilityCaster.abilities[playerAbilityCaster.activeIndex].abilityName;
    }*/
}
