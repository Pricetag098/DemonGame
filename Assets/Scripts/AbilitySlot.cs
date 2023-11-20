using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilitySlot : MonoBehaviour
{
    public Ability ability;

    [SerializeField] GameObject lockImage;

    [SerializeField] TextMeshProUGUI AbilityIcon;

    [SerializeField] Color selectedColour;

    [SerializeField] Color hasColour;

    [SerializeField] Color doesntHaveColour;

    public GameObject scanZone;
    [SerializeField] TMP_FontAsset blankFont;
    [SerializeField] TMP_FontAsset colourFont;

    public bool hasAbility;

    public bool selected;

    Image sectionImage;

    WeaponWheel weaponWheel;

    public void HasAbility()
    {
        hasAbility = true;

        AbilityIcon.font = colourFont;
        lockImage.SetActive(false);
        sectionImage.color = hasColour;
        scanZone.SetActive(true);
    }
    public void DoesntHaveAbility()
    {
        hasAbility = false;

        AbilityIcon.font = blankFont;
        lockImage.SetActive(true);
        scanZone.SetActive(false);
        sectionImage.color = doesntHaveColour;
    }

    public void OnSelect()
    {
        selected = true;

        sectionImage.color = selectedColour;

        weaponWheel.AbilitySelected(ability);
    }

    public void OnDeselect()
    {
        selected = false;

        if (hasAbility)
        {
            sectionImage.color = hasColour;
        }
        else
        {
            sectionImage.color = doesntHaveColour;
        }
    }

    private void Awake()
    {
        sectionImage = GetComponent<Image>();

        weaponWheel = GetComponentInParent<WeaponWheel>();

        AbilityIcon.text = ability.symbolText;

        DoesntHaveAbility();
        OnDeselect();
    }
}
