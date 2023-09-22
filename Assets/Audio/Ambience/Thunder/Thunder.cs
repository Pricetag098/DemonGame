using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Thunder : MonoBehaviour
{
    public AudioClip[] thunderSounds;

    public float minIntensity = 0.5f;
    public float maxIntensity = 3f;
    public float intensityIncreaseSpeed = 5f;
    public float intensityDecreaseSpeed = 2f;
    public float minFlickerIntensity = 0.2f;
    public float maxFlickerIntensity = 0.5f;
    public float flickerSpeed = 10f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 5f;
    public int simultaneousStrikes = 3;


    private Light directionalLight;
    private Material skyboxMaterial;
    private AudioSource audioSource;
    private bool isFlashing = false;
    private float targetIntensity;
    private float currentIntensity;
    private float targetExposure; 
    private float currentExposure; 
    private float exposureLerpDuration = 0.2f;

    private void Awake()
    {
        directionalLight = GetComponent<Light>();
        audioSource = GetComponent<AudioSource>();
        skyboxMaterial = RenderSettings.skybox;

    }

    private void Start()
    {
        StartCoroutine(FlashLightning());
    }

    private IEnumerator FlashLightning()
    {
        while (true)
        {
            for (int i = 0; i < simultaneousStrikes; i++)
            {
                float waitTime = Random.Range(minWaitTime, maxWaitTime);

                yield return new WaitForSeconds(waitTime);

                isFlashing = true;
                targetIntensity = Random.Range(minIntensity, maxIntensity);
                targetExposure = 8f; 
                PlayThunderSound();

               
                while (currentIntensity < targetIntensity)
                {
                    currentIntensity += intensityIncreaseSpeed * Time.deltaTime;
                    directionalLight.intensity = currentIntensity;

                    float flickerIntensity = Mathf.Lerp(maxFlickerIntensity, minFlickerIntensity, Mathf.PingPong(Time.time * flickerSpeed, 1f));
                    directionalLight.intensity += flickerIntensity;

                    float t = Mathf.Clamp01((currentIntensity - minIntensity) / (targetIntensity - minIntensity));
                    currentExposure = Mathf.Lerp(1.8f, targetExposure, t); 
                    //skyboxMaterial.SetFloat("_Exposure", currentExposure); 

                    yield return null;
                }

                yield return new WaitForSeconds(exposureLerpDuration);

                
                while (currentIntensity > minIntensity)
                {
                    currentIntensity -= intensityDecreaseSpeed * Time.deltaTime;
                    directionalLight.intensity = currentIntensity;

                    float t = Mathf.Clamp01((currentIntensity - targetIntensity) / (minIntensity - targetIntensity));
                    currentExposure = Mathf.Lerp(targetExposure, 1.8f, t); 
                    //skyboxMaterial.SetFloat("_Exposure", currentExposure); 

                    yield return null;
                }

                isFlashing = false;
            }
        }
    }

    private void PlayThunderSound()
    {
        if (audioSource != null && thunderSounds != null && thunderSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, thunderSounds.Length);
            AudioClip randomThunderSound = thunderSounds[randomIndex];
            audioSource.PlayOneShot(randomThunderSound);
        }
    }
}
