using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBob : MonoBehaviour
{

    public LineRenderer _lineRenderer;

    [SerializeField] private Transform[] points;


    // Start is called before the first frame update
    public void Start()
    {
        _lineRenderer.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    public void Update()
    {
        _lineRenderer.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            _lineRenderer.SetPosition(i, points[i].position);
        }
    }
}
