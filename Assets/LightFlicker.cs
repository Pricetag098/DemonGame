using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light torchLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 1.0f;

    private float currentIntensity;
    private float timeCounter = 0.0f;

    private void Start()
    {
        if (torchLight == null)
        {
            Debug.LogError("Light component is not assigned.");
            enabled = false;
            return;
        }

        currentIntensity = Random.Range(minIntensity, maxIntensity);
    }

    private void Update()
    {
        // Use a sine wave to smoothly transition the light intensity
        timeCounter += Time.deltaTime * flickerSpeed;
        float flickerValue = Mathf.Sin(timeCounter);
        float targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, (flickerValue + 1) / 2);

        // Update the light intensity
        torchLight.intensity = targetIntensity;
    }
}
