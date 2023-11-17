using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class WeaponInfo : MonoBehaviour
{
    [SerializeField] RectTransform nameParent;
    [SerializeField] RectTransform reserveNameParent;
    [SerializeField] public TextMeshProUGUI reserveNameText;
    bool switchtext = false;
    public AnimationCurve easeCurve;

    [SerializeField] TextMeshProUGUI clipAmountText;
    [SerializeField] TextMeshProUGUI stashAmountText;
    [SerializeField] TextMeshProUGUI dividerText;
    [SerializeField] public TextMeshProUGUI nameText;
    [SerializeField] CanvasGroup noAmmoNotificaiton;
    [SerializeField] Holster holster;


    [SerializeField] Color defaultColour;
    [SerializeField] Color noAmmoColour;

    bool displayOpen;
    private void Awake()
    {
        holster = FindObjectOfType<Holster>();
        displayOpen = noAmmoNotificaiton.alpha == 1;
    }

    void Update()
    {
        nameText.text = holster.HeldGun.gunName;
        if(holster.gunCount > 1) { reserveNameText.text = holster.OffHandGun.gunName; }
        clipAmountText.text = holster.HeldGun.ammoLeft.ToString();
        stashAmountText.text = holster.HeldGun.stash.ToString();
        clipAmountText.color = defaultColour;
        stashAmountText.color = defaultColour;
        dividerText.color = defaultColour;
        

        if (holster.HeldGun.ammoLeft == 0)
        {
            clipAmountText.color = noAmmoColour;
        }
        
        if (holster.HeldGun.stash == 0)
        {
            stashAmountText.color = noAmmoColour;
        }
        
        if (holster.HeldGun.ammoLeft == 0 && holster.HeldGun.stash == 0)
        {
            dividerText.color = noAmmoColour;
            if (!displayOpen)
            {
                noAmmoNotificationTween(1);
                displayOpen = true;
            }
            
        }
        else
        {
            if(displayOpen)
            {
                noAmmoNotificationTween(0);
                displayOpen = false;
            }
            
        }
    }

    public void WeaponSwapTween()
    {
        
        if (!switchtext)
        {
            WeaponSwitchTween(reserveNameParent, nameParent);

            switchtext = true;
        }
        else
        {
            WeaponSwitchTween(nameParent, reserveNameParent);

            switchtext = false;
        }
    }

    private void noAmmoNotificationTween(int alphaValue)
    {
        noAmmoNotificaiton.DOFade(alphaValue, 0.2f);
    }

    void WeaponSwitchTween(RectTransform reserveText, RectTransform activeText)
    {
        if (!switchtext)
        {
            ActiveText(reserveNameText);
            ReserveText(nameText);
        }
        else
        {
            ActiveText(nameText);
            ReserveText(reserveNameText);
        }

        reserveText.DOAnchorPos(new Vector2(-30, 40), holster.drawTimer).SetEase(easeCurve);

        activeText.DOAnchorPos(new Vector2(0, 90), holster.drawTimer).SetEase(easeCurve);
    }

    void ReserveText(TextMeshProUGUI reserveText)
    {
        reserveText.fontSize = 45;
        reserveText.color = Color.grey;
    }

    void ActiveText(TextMeshProUGUI activeText)
    {
        activeText.fontSize = 50;
        activeText.color = Color.white;
    }
}
