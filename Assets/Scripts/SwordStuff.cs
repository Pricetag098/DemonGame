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

    [SerializeField] List<Material> swordMaterials;
    [SerializeField] List<Renderer> swordRenderers;
    [SerializeField] Renderer hiltRen;

    Vector3 originalPos;
    Material dissolve;
    Material hiltMaterial;

    private void Awake()
    {
        originalPos = sword.transform.localPosition;
        dissolve = swordRenderers[0].sharedMaterial;
        hiltMaterial = hiltRen.sharedMaterial;
    }

    private void Start()
    {
        dissolve.SetFloat("_Alpha_Clip", 1);
        hiltMaterial.SetFloat("_Alpha_Clip", 1);
    }

    public void EquipSword()
    {
        sword.SetActive(true);
        sword.transform.localPosition = equipPos;
        Sequence equip = DOTween.Sequence();
        equip.AppendCallback(() => 
        { 
            DOTween.To(() => dissolve.GetFloat("_Alpha_Clip"), x => dissolve.SetFloat("_Alpha_Clip", x), 0, dissolveTime);
            DOTween.To(() => hiltMaterial.GetFloat("_Alpha_Clip"), x => hiltMaterial.SetFloat("_Alpha_Clip", x), 0, dissolveTime);
        });
        equip.Append(sword.transform.DOLocalMove(originalPos, returnTime));
    }

    public void UnEquipSword()
    {
        Sequence unEquip = DOTween.Sequence();
        unEquip.Append(sword.transform.DOLocalMove(unEquipPos, moveTime));
        unEquip.AppendCallback(() => 
        {
            DOTween.To(() => dissolve.GetFloat("_Alpha_Clip"), x => dissolve.SetFloat("_Alpha_Clip", x), 1, dissolveTime);
            DOTween.To(() => hiltMaterial.GetFloat("_Alpha_Clip"), x => hiltMaterial.SetFloat("_Alpha_Clip", x), 1, dissolveTime);
        });
        unEquip.AppendInterval(dissolveTime);
        unEquip.AppendCallback(() => sword.transform.localPosition = originalPos);
        unEquip.AppendCallback(() => sword.SetActive(false));
    }


    public void UpdateMat(int tier)
    {
        
        foreach(Renderer child in swordRenderers)
        {
            child.sharedMaterial = swordMaterials[tier];
        }
        if (dissolve.GetFloat("_Aplha_Clip") == 1)
        {
            swordMaterials[tier].SetFloat("_Alpha_Clip", 1);
        }
        else
        {
            swordMaterials[tier].SetFloat("_Alpha_Clip", -1);
        }
        dissolve = swordMaterials[tier];

    }
}
