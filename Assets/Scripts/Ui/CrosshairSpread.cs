using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CrosshairSpread : MonoBehaviour
{
    [SerializeField] AnimationCurve recoilSpreadCurve;
    Holster holster;
    RectTransform RectTransform;
    // Start is called before the first frame update
    void Start()
    {
        RectTransform = GetComponent<RectTransform>();
        holster = FindObjectOfType<Holster>();
    }

    // Update is called once per frame
    void Update()
    {
        float val = recoilSpreadCurve.Evaluate(holster.HeldGun.GetSpread(Vector3.up).magnitude);
        RectTransform.sizeDelta = Vector2.one * val;
    }
}
