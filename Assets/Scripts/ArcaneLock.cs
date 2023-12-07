using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.VFX;

public class ArcaneLock : MonoBehaviour
{
    public GameObject border;

    public GameObject key;

    public VisualEffect lockedEffected;

    public VisualEffect unlockedEffect;

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
        borderMat.SetFloat("_AlphaClip", 1);
        keyMat.SetFloat("_AlphaClip", 1);
        unlockedEffect.Stop();
        lockedEffected.Play();
    }

    public void DisolveLock()
    {
        Sequence dissolve = DOTween.Sequence();

        dissolve.Append(border.transform.DOLocalRotate(new Vector3(0, 0, -45f), turnTime));
        dissolve.AppendInterval(turnTime);
        dissolve.AppendCallback(() => 
        {
            unlockedEffect.Play();            
            lockedEffected.Stop();
        });
        dissolve.Append(DOTween.To(() => borderMat.GetFloat("_AlphaClip"), x => borderMat.SetFloat("_AlphaClip", x), 0, dissolveTime));
        dissolve.Join(DOTween.To(() => keyMat.GetFloat("_AlphaClip"), x => keyMat.SetFloat("_AlphaClip", x), 0, dissolveTime));
        dissolve.AppendCallback(() => unlockedEffect.Stop());
    }

    public void NoBuy()
    {
        transform.DOPunchScale(punchScale, punchTime, punchVibrato, punchElasticity);
    }
}
