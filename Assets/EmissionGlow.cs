using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionGlow : MonoBehaviour
{
    public float maxEmission;
    public float minEmission;

    public Color origionalColour;

    private Material material;

    private void Awake()
    {
        material = GetComponent <Material>();
        origionalColour = material.GetColor("_EmissionColour");
    }





}
