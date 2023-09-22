using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
public class HealthScreenEffect : MonoBehaviour
{
    [SerializeField] VolumeProfile profile;
    [SerializeField] float maxVinette = 1;
    [SerializeField] Health health;

    // Update is called once per frame
    void Update()
    {
        Vignette vignette;
        if (profile.TryGet(out vignette))
        {
            float vinetteVal = (1 - (health.health / health.maxHealth)) * maxVinette;
            vignette.intensity.value = vinetteVal * maxVinette;
        }
    }
}
