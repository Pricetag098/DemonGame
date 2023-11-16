using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordStuff : MonoBehaviour
{
    [SerializeField] float dissolveTime;
    [SerializeField] float moveTime;
    [SerializeField] float returnTime;
    [SerializeField] Vector3 unEquipPos;
    [SerializeField] Vector3 equipPos;
    [SerializeField] GameObject sword;

    Vector3 originalPos;
    Material dissolve;
    TrailRenderer trailRenderer;

    private void Awake()
    {
        originalPos = sword.transform.localPosition;
        dissolve = sword.GetComponentInChildren<Renderer>().sharedMaterial;
        trailRenderer = sword.GetComponentInChildren<TrailRenderer>();
        dissolve.SetFloat("_Alpha_Clip", 1);
        trailRenderer.enabled = false;
    }

    public void Equip()
    {
        sword.SetActive(true);
        sword.transform.localPosition = equipPos;
        Sequence equip = DOTween.Sequence();
        equip.Append(DOTween.To(() => dissolve.GetFloat("_Alpha_Clip"), x => dissolve.SetFloat("_Alpha_Clip", x), -1, dissolveTime));
        equip.Append(sword.transform.DOLocalMove(originalPos, returnTime));
    }

    public void UnEquip()
    {
        Sequence unEquip = DOTween.Sequence();
        unEquip.Append(sword.transform.DOLocalMove(unEquipPos, moveTime));
        unEquip.Append(DOTween.To(() => dissolve.GetFloat("_Alpha_Clip"), x => dissolve.SetFloat("_Alpha_Clip", x), 1, dissolveTime));
        unEquip.AppendInterval(dissolveTime);
        unEquip.AppendCallback(() => sword.transform.localPosition = originalPos);
        unEquip.AppendCallback(() => sword.SetActive(false));
    }

    public void TrailOn()
    {
        trailRenderer.enabled = true;
    }

    public void TrailOff()
    {
        trailRenderer.enabled = false;
    }
}
