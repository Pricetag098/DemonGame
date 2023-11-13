using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class FlashImage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<TextMeshProUGUI>().DOFade(0.25f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}
