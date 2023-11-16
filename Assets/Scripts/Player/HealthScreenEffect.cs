using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
public class HealthScreenEffect : MonoBehaviour
{
    [SerializeField] Volume profile;
    [SerializeField] float maxVinette = 1;
    [SerializeField] Health health;

    Vignette vignette;

    // Update is called once per frame
    private void Awake()
    {
        Vignette vignette1;
        if (profile.profile.TryGet(out vignette1))
        {
            vignette = vignette1;
        }
    }

    void Update()
    {
        float vinetteVal = (1 - (health.health / health.maxHealth)) * maxVinette;
        vignette.intensity.value = vinetteVal;
    }
}
