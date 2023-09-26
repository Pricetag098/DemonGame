using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FlashImage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Image>().DOFade(0.25f, 1f).SetEase(Ease.InOutBack).SetLoops(-1, LoopType.Yoyo);
    }
}
