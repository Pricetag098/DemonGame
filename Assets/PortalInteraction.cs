using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalInteraction : MonoBehaviour
{
    [SerializeField] GameObject portal;

    [SerializeField] Vector3 minPortalSize;

    [SerializeField] Vector3 maxPortalSize;

    [SerializeField] Animator armAnimator;

    [SerializeField] float minEmission;

    [SerializeField] float maxEmission;

    [SerializeField] float time;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        portal.transform.DOScale(maxPortalSize, time);
    }

    private void OnTriggerExit(Collider other)
    {

    }
}
