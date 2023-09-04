using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    Vector3 scale;
    [SerializeField] AnimationCurve sizeOverLifetime;
    float time;
    [SerializeField] float lifeTime = 1f;

    public void Spawn(Vector3 scale)
	{
        this.scale = scale;
        time = 0;
        transform.localScale = Vector3.zero;
	}

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(Vector3.zero, scale, sizeOverLifetime.Evaluate(time/lifeTime));
        time += Time.deltaTime;
    }
}
