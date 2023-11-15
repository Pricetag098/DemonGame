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
    }

    public void SpawnAbility()
    {
        DOTween.Kill(this, true);
        DOTween.To(() => dissolveMat.GetFloat("_Alpha_Clip"), x => dissolveMat.SetFloat("_Alpha_Clip", x), 0, dissolveTime);
    }

    public void DespawnAbility()
    {
        DOTween.Kill(this, true);
        DOTween.To(() => dissolveMat.GetFloat("_Alpha_Clip"), x => dissolveMat.SetFloat("_Alpha_Clip", x), 1, dissolveTime);
    }
}
