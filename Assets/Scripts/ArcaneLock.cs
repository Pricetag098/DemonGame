using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

public class ArcaneLock : MonoBehaviour
{
    public GameObject border;

    public GameObject key;

    public float turnTime;

    public float dissolveTime;

    public float punchTime;

    public int punchVibrato = 10;

    public int punchElasticity = 1;

    public Vector3 punchScale;

    private Material borderMat;

    private Material keyMat;

    private void Awake()
    {
        borderMat = border.GetComponent<Renderer>().material;
        keyMat = key.GetComponent<Renderer>().material;
    }

    private void Start()
    {
        borderMat.SetFloat("_Dissolve_Amount", 0);
        keyMat.SetFloat("_Dissolve_Amount", 0);
    }

    public void DisolveLock()
    {
        Sequence dissolve = DOTween.Sequence();

        dissolve.Append(border.transform.DOLocalRotate(new Vector3(0, 0, -45f), turnTime));
        dissolve.AppendInterval(turnTime);
        dissolve.Append(DOTween.To(() => borderMat.GetFloat("_Dissolve_Amount"), x => borderMat.SetFloat("_Dissolve_Amount", x), 1, dissolveTime));
        dissolve.Join(DOTween.To(() => keyMat.GetFloat("_Dissolve_Amount"), x => keyMat.SetFloat("_Dissolve_Amount", x), 1, dissolveTime));
    }

    public void NoBuy()
    {
        transform.DOPunchScale(punchScale, punchTime, punchVibrato, punchElasticity);
    }
}
