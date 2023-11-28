using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionGlow : MonoBehaviour
{
    public float maxEmission;
    public float minEmission;
    public float blinkTime;

    private Color originalColour;

    private Material material;

    float intensity;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        originalColour = material.GetColor("_EmissionColour");
    }

    private void Start()
    {
        Blink(true);
    }

    private void Update()
    {
        Color newEmissionColor = originalColour * intensity;

        material.SetColor("_EmissionColour", newEmissionColor);

        if(intensity == maxEmission)
        {
            Blink(false);
        }
        else if(intensity == minEmission)
        {
            Blink(true);
        }
    }

    private void Blink(bool up)
    {
        if (up)
        {
            DOTween.To(() => intensity, x => intensity = x, maxEmission, blinkTime);
        }
        else
        {
            DOTween.To(() => intensity, x => intensity = x, minEmission, blinkTime);
        }
    }

}
