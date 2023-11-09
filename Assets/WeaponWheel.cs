using DG.Tweening;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using TMPro;

public class WeaponWheel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI selectedAbilityIcon;
    [SerializeField] TextMeshProUGUI selectedAbilityName;

    private bool open;
    CanvasGroup canvasGroup;
    [SerializeField] float openTime;
    [SerializeField][Range(0, 1)] float timeScale;
    [SerializeField] float timeUntilKickout;
    [SerializeField] float kickOutTime;

    [SerializeField] InputActionProperty tabAction;

    [SerializeField] List<AbilitySlot> abilitySlots = new List<AbilitySlot>();

    [SerializeField] Volume blackWhiteVolume;

    [SerializeField] Volume normalVolume;

    private AbilityCaster caster;

    float kickoutTimer;

    private void Awake()
    {
        caster = FindObjectOfType<AbilityCaster>();
        canvasGroup = GetComponent<CanvasGroup>();
        open = true;
        Close(openTime);
        tabAction.action.performed += Open;
        kickoutTimer = timeUntilKickout *  timeScale;
    }

    public void AbilitySelected(Ability ability)
    {
        selectedAbilityIcon.text = ability.fontReference.ToString();
        selectedAbilityName.text = ability.name.ToString();
    }

    public void GainedAbility(Ability ability)
    {
        foreach (AbilitySlot abilitySlot in abilitySlots)
        {
            if(abilitySlot.ability.name == ability.name)
            {
                abilitySlot.HasAbility();
                return;
            }
        }
    }

    public void Open(InputAction.CallbackContext context)
    {
        if (open)
            return;
        DOTween.Kill(this, true);
        open = true;
        Sequence f = DOTween.Sequence(this);
        f.SetUpdate(true);
        f.Append(canvasGroup.DOFade(1, openTime));
        f.AppendCallback(() => DOTween.To(() => Time.timeScale, x => Time.timeScale = x, timeScale, openTime));
        f.AppendCallback(() => DOTween.To(() => Time.fixedDeltaTime, x => Time.fixedDeltaTime = x, 0.01f * timeScale, openTime));
        f.AppendCallback(() => DOTween.To(() => blackWhiteVolume.weight, x => blackWhiteVolume.weight = x, 1f, openTime));
        f.AppendCallback(() => DOTween.To(() => normalVolume.weight, x => normalVolume.weight = x, 0f, openTime));
        f.AppendCallback(() =>
        {
            canvasGroup.interactable = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        });
    }

    public void Close(float time)
    {
        foreach (var item in abilitySlots)
        {
            if (item.selected)
            {
                for (int i = 0; i < caster.abilities.Length; i++)
                {
                    if (caster.abilities[i].name == item.ability.name)
                    {
                        caster.abilities[i].Equip(caster);
                    }
                }
            }
        }

        if (!open)
            return;
        DOTween.Kill(this, true);
        open = false;
        Sequence f = DOTween.Sequence(this);
        f.SetUpdate(true);
        f.Append(canvasGroup.DOFade(0, time));
        f.AppendCallback(() => DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, time));
        f.AppendCallback(() => DOTween.To(() => Time.fixedDeltaTime, x => Time.fixedDeltaTime = x, 0.01f, time));
        f.AppendCallback(() => DOTween.To(() => blackWhiteVolume.weight, x => blackWhiteVolume.weight = x, 0f, time));
        f.AppendCallback(() => DOTween.To(() => normalVolume.weight, x => normalVolume.weight = x, 1f, time));
        f.AppendInterval(time);
        f.AppendCallback(() =>
        {
            canvasGroup.interactable = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        });

        kickoutTimer = timeUntilKickout * timeScale;
    }

    private void Update()
    {
        if (open)
        {
            kickoutTimer -= Time.deltaTime;

            if(kickoutTimer < 0)
            {
                Close(kickOutTime);
            }

            if (tabAction.action.WasReleasedThisFrame())
            {
                Close(openTime);
            }
        }
    }

    private void OnEnable()
    {
        tabAction.action.Enable();
    }
    private void OnDisable()
    {
        tabAction.action.Disable();
    }
    private void OnDestroy()
    {
        tabAction.action.performed -= Open;
    }
}
