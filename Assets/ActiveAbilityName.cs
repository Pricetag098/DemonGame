using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.TextCore.Text;

public class ActiveAbilityName : MonoBehaviour
{
    [SerializeField] FontAsset[] fontList;
    FontAsset currentFont;

    [SerializeField] TextMeshProUGUI abilityNameText;

    void Awake()
    {
        //abilityNameText.font;
    }

    public void OnSwitchAbility()
    {
        currentFont = fontList[0];
    }
}
