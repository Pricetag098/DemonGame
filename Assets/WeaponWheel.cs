using DG.Tweening;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using TMPro;
using Movement;

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

    public InputActionProperty mousePos;

    private PlayerAbilityCaster caster;

    PlayerInputt playerInput;

    float kickoutTimer;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInputt>();
        caster = FindObjectOfType < PlayerAbilityCaster>();
        canvasGroup = GetComponent<CanvasGroup>();
        open = true;
        Close(openTime);
        tabAction.action.performed += Open;
        kickoutTimer = timeUntilKickout *  timeScale;
    }

    public void AbilitySelected(Ability ability)
    {
        selectedAbilityIcon.text = ability.fontReference.ToString();
        selectedAbilityName.text = ability.abilityName.ToString();
    }

    public void GainedAbility(Ability ability)
    {
        foreach (AbilitySlot abilitySlot in abilitySlots)
        {
            if(abilitySlot.ability.guid == ability.guid)
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
        playerInput.tabing = true;
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
            Cursor.lockState = CursorLockMode.Confined;
        });
    }

    public void Close(float time)
    {
        foreach (var item in abilitySlots)
        {
            if (item.selected)
            {
                for (int i = 0; i < caster.caster.abilities.Length; i++)
                {
                    if (caster.caster.abilities[i].guid == item.ability.guid)
                    {
						caster.SelectAbility(i);
                    }
                }
            }
        }

        if (!open)
            return;
        DOTween.Kill(this, true);
        open = false;
        playerInput.tabing = false;
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

            FindTarget();
        }
    }

    public void FindTarget()
    {
        int bestSlot = -1;
        float bestValue = float.NegativeInfinity;


        for(int i = 0;i < abilitySlots.Count;i++)
        { 
            Vector3 toScanZone = (abilitySlots[i].scanZone.GetComponent<RectTransform>().position - GetComponent<RectTransform>().position);
            //Debug.Log(toScanZone  + " Scan Zone",);

            Vector3 toMouse = ((Vector3)mousePos.action.ReadValue<Vector2>() - new Vector3(Screen.width / 2, Screen.height / 2));
            //Debug.Log(toMouse + " To Mouse");
            float value = Vector3.Dot(toMouse, toScanZone);

			if (value > bestValue && abilitySlots[i].hasAbility)
            {
                bestSlot = i;
                bestValue = value;
            }
        }
        for(int i = 0; i < abilitySlots.Count; i++)
        {
            if(i == bestSlot)
            {
                abilitySlots[i].OnSelect();
            }
            else
            {
                abilitySlots[i].OnDeselect();
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    AbilitySlot bestSlot = null;
    //    float bestValue = float.NegativeInfinity;

    //    foreach (AbilitySlot abilitySlot in abilitySlots)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(transform.position, 0.1f);
    //        Vector3 toScanZone = abilitySlot.scanZone.transform.position - transform.position;

    //        Gizmos.DrawWireSphere(abilitySlot.scanZone.transform.position, 0.1f);
    //        Vector3 toMouse = mousePos.action.ReadValue<Vector2>() - new Vector2(Screen.width / 2, Screen.height / 2);
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawWireSphere(mousePos.action.ReadValue<Vector2>(), 0.1f);
    //        Gizmos.DrawWireSphere(new Vector2(Screen.width / 2, Screen.height / 2), 0.1f);
    //        if (Vector3.Dot(toMouse, toScanZone) > bestValue)
    //        {
    //            bestSlot = abilitySlot;
    //        }
    //    }

    //    bestSlot.OnSelect();
    //}

    private void OnEnable()
    {
        tabAction.action.Enable();
        mousePos.action.Enable();
    }
    private void OnDisable()
    {
        tabAction.action.Disable();
        mousePos.action.Disable();
    }
    private void OnDestroy()
    {
        tabAction.action.performed -= Open;
    }
}
