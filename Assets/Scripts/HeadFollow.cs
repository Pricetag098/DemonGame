using FIMSpace.FLook;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadFollow : MonoBehaviour
{
    [SerializeField] Transform playerHead;
    private FLookAnimator lookAnimatior;

    private void Awake()
    {
        lookAnimatior = GetComponent<FLookAnimator>();
        playerHead = FindObjectOfType<PlayerHead>().transform;
        lookAnimatior.ObjectToFollow = playerHead;
    }

    private void Start()
    {
        lookAnimatior.ObjectToFollow = playerHead;
    }
}
