using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public float openTime;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        //canvasGroup.alpha = 1;
        canvasGroup.DOFade(1,openTime).OnComplete(() => { canvasGroup.interactable = true; canvasGroup.blocksRaycasts = true; }).SetUpdate(true);
    }
    public void Close()
    {
        canvasGroup.DOFade(0, openTime).OnComplete(() => { canvasGroup.interactable = false; canvasGroup.blocksRaycasts = false; }).SetUpdate(true);
    }
}
