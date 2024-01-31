using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossDissolve : MonoBehaviour
{
    public List<Renderer> crossParts;

    public float dissolveTime;

    private List<Material> crossMats = new List<Material>();

    private void Awake()
    {
        foreach (Renderer part in crossParts)
        {
            crossMats.Add(part.material);
        }
    }

    public void On()
    {
        DOTween.Kill(this);
        foreach (Material item in crossMats)
        {
            DOTween.To(() => item.GetFloat("_Dissolve_Amount"), x => item.SetFloat("_Dissolve_Amount", x), -1, dissolveTime);
        }
    }

    public void Off()
    {
        DOTween.Kill(this);
        foreach (Material item in crossMats)
        {
            DOTween.To(() => item.GetFloat("_Dissolve_Amount"), x => item.SetFloat("_Dissolve_Amount", x), 1, dissolveTime);
        }
    }
}
