using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlyphSpawning : MonoBehaviour
{
    public GameObject abilityGlyph;

    public float dissolveTime;

    private Material dissolveMat;

    private void Awake()
    {
        dissolveMat = abilityGlyph.GetComponentInChildren<Renderer>().sharedMaterial;
        Debug.Log(dissolveMat.name);
    }

    public void SpawnAbility()
    {
        DOTween.To(() => dissolveMat.GetFloat("_Alpha_Clip"), x => dissolveMat.SetFloat("_Alpha_Clip", x), 0, dissolveTime);
        Debug.Log("out");
    }

    public void DespawnAbility()
    {
        DOTween.To(() => dissolveMat.GetFloat("_Alpha_Clip"), x => dissolveMat.SetFloat("_Alpha_Clip", x), 1, dissolveTime);
        Debug.Log("in");
    }
}
