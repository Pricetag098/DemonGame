using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalFrame : MonoBehaviour
{
    [SerializeField] float maxEmission = 1.0f;
    [SerializeField] float minEmission = 0.0f;
    [SerializeField] float blinkSpeed = 1.0f;

    private Material material;
    private Color originalEmissionColor;
    private float currentEmissionIntensity;
    private bool isBlinking = false;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
        originalEmissionColor = material.GetColor("_EmissionColour");
    }

    private void Start()
    {
        StartEmissionBlink();
    }

    private void Update()
    {
        if (isBlinking)
        {
            float emissionIntensity = Mathf.PingPong(Time.time * blinkSpeed, maxEmission - minEmission) + minEmission;

            currentEmissionIntensity = emissionIntensity;

            Color newEmissionColor = originalEmissionColor * emissionIntensity;

            material.SetColor("_EmissionColour", newEmissionColor);
        }
        else
        {
            currentEmissionIntensity = Mathf.Lerp(currentEmissionIntensity, maxEmission, Time.deltaTime * blinkSpeed);

            Color newEmissionColor = originalEmissionColor * currentEmissionIntensity;

            material.SetColor("_EmissionColour", newEmissionColor);

            if (Mathf.Approximately(currentEmissionIntensity, maxEmission))
            {
                currentEmissionIntensity = maxEmission;
            }
        }
    }

    public void StartEmissionBlink()
    {
        isBlinking = true;
    }

    public void StopEmissionBlink()
    {
        isBlinking = false;
    }
}

