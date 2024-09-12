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
        //Sequence unEquip = DOTween.Sequence();
        //unEquip.Append(sword.transform.DOLocalMove(unEquipPos, moveTime));
        //unEquip.AppendCallback(() => sword.transform.localPosition = originalPos);
        //unEquip.AppendCallback(() => sword.SetActive(false));
        sword.SetActive(false);
        sword.transform.localPosition = originalPos;
    }


    public void UpdateMat(int tier)
    {
        
        foreach(Renderer child in swordRenderers)
        {
            child.sharedMaterial = swordMaterials[tier];
        }
        dissolve = swordMaterials[tier];
    }
}
