using DG.Tweening;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class WeaponWheel : MonoBehaviour
{
    private bool open;
    CanvasGroup canvasGroup;
    [SerializeField] float openTime;
    [SerializeField][Range(0, 1)] float timeScale;

    [SerializeField] InputActionProperty tabAction;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        open = true;
        Close();
        tabAction.action.performed += Open;
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
        f.AppendCallback(() =>
        {
            canvasGroup.interactable = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        });
    }

    public void Close()
    {
        if (!open)
            return;
        DOTween.Kill(this, true);
        open = false;
        Sequence f = DOTween.Sequence(this);
        f.SetUpdate(true);
        f.AppendCallback(() =>
        {
            canvasGroup.interactable = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        });
        f.Append(canvasGroup.DOFade(0, openTime));
        f.AppendCallback(() => DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, openTime));
        f.AppendCallback(() => DOTween.To(() => Time.fixedDeltaTime, x => Time.fixedDeltaTime = x, 0.01f, openTime));
    }

    private void Update()
    {
        if (open)
        {
            if (tabAction.action.WasReleasedThisFrame())
            {
                Close();
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
