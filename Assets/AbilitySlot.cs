using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    public Ability ability;

    [SerializeField] GameObject lockImage;

    [SerializeField] Color selectedColour;

    [SerializeField] Color hasColour;

    [SerializeField] Color doesntHaveColour;

    Image sectionImage;

    public bool hasAbility
    {
        get {  return hasAbility; }
        set 
        {
            hasAbility = value; 
            if(value)
            {
                lockImage.SetActive(false);
                sectionImage.color = hasColour;
                
            }
            else
            {
                lockImage.SetActive(true);
                sectionImage.color = doesntHaveColour;
            }
        }
    }

    public bool selected
    {
        get { return selected; }
        set
        {
            selected = value;
            if (value)
            {
                sectionImage.color = selectedColour;
            }
            else
            {
                if (hasAbility)
                {
                    sectionImage.color = hasColour;
                }
                else
                {
                    sectionImage.color = doesntHaveColour;
                }
            }
        }
    }

    private void Start()
    {
        sectionImage = GetComponent<Image>();
    }
}
