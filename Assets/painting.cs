using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class painting : MonoBehaviour
{
    [SerializeField] SoundPlayer badSound;
    [SerializeField] SoundPlayer goodSound;

    [SerializeField] CanvasGroup sigil;

    public string sigilLetter;

    BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public void ResetPainting()
    {
        boxCollider.enabled = true;
        FadeCanvasGroup(sigil, 1, 0, 1f);
    }

    public void PaintingBad()
    {
        badSound.Play();
        sigil.alpha = 1;
        FadeCanvasGroup(sigil, 1, 0, 1f);
    }

    public void PaintingGood()
    {
        goodSound.Play();
        FadeCanvasGroup(sigil, 0, 1, 0.5f);
        boxCollider.enabled = false;
    }

    public void FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        StartCoroutine(FadeCoroutine(canvasGroup, startAlpha, endAlpha, duration));
    }

    // Coroutine to handle the fade
    private IEnumerator FadeCoroutine(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = startAlpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null; // Wait for the next frame
        }

        // Ensure the final alpha value is set
        canvasGroup.alpha = endAlpha;
    }
}
