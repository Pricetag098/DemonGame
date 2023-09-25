using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class MenuLoad : MonoBehaviour
{
    public GameObject[] panels;

    public List<FadeInTween> fade;

    public float initialWait;
    public float delayTime;

    float time = 0;
    bool counting = true;
    bool set = false;

    private void Start()
    {
        StartCoroutine(WaitForDramaticEffect());
    }

    private void Update()
    {
        if (counting) { time += Time.deltaTime; }

        if (time > initialWait)
        {
            counting = false;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                baseActive();
            }
        }

    }

    IEnumerator WaitForDramaticEffect()
    {
        yield return new WaitForSeconds(initialWait);

        fade[0].TweenIn();

        yield return new WaitForSeconds(delayTime);

        fade[1].TweenIn();

        yield return new WaitForSeconds(delayTime);

        set = true;
    }

    public void baseActive()
    {
        fade[0].canvasGroup.alpha = 1f;
        fade[1].canvasGroup.alpha = 1f;

        StopAllCoroutines();

        if (set)
        {
            panels[0].SetActive(false);
            panels[1].SetActive(true);
        }

        set = true;
    }

}
