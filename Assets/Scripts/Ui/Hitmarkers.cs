using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitmarkers : MonoBehaviour
{
    [SerializeField] ObjectPooler baseMarkers, critMarkers;
    [SerializeField] float showTime,decayTime;
    [SerializeField] float punchScale = 1.1f;
    private void Awake()
    {
        FindObjectOfType<Holster>().OnDealDamage += OnHit;
    }


    void OnHit(float damage,HitBox hitBox)
    {
        if(hitBox.bodyPart== HitBox.BodyPart.Head || hitBox.bodyPart == HitBox.BodyPart.Crit)
        {
            SpawnHitMarker(critMarkers.Spawn());
        }
        else
        {
            SpawnHitMarker(baseMarkers.Spawn());
        }
    }

    void SpawnHitMarker(GameObject go)
    {
        DOTween.Kill(go,true);
        go.GetComponent<SoundPlayer>().Play();
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        
        CanvasGroup group = go.GetComponent<CanvasGroup>();
        group.alpha = 1.0f;

        Sequence s = DOTween.Sequence(go);
        s.Append(rectTransform.DOPunchScale(Vector3.one * punchScale, showTime));
        s.Append(group.DOFade(0,decayTime));

    }
}
