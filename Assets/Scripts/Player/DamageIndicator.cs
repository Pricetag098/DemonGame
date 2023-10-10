using Autodesk.Fbx;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    [SerializeField] private List<RectTransform> indicators;
    [SerializeField] private List<float> startTimes;
    [SerializeField] private float delay;
    private int value;
    private Quaternion tRotation;
    // Start is called before the first frame update

    private void Start()
    {
        value = 0;
        foreach (RectTransform rectTransform in indicators)
        {
            startTimes.Add(-delay);
        }
    }

    private void Update()
    {
        foreach (float time in startTimes)
        {
            indicators[startTimes.IndexOf(time)].GetComponent<Image>().color = new Color ( 1, 1, 1, (delay - (Time.time - time)) / delay);
            //indicators[startTimes.IndexOf(time)].GetComponentInChildren<Image>().color = new Color(1, 1, 1, (delay - (Time.time - time)) / delay);

        }
    }

    public void Indicate(Transform enemy)
    {
        Vector3 direction = orientation.position - enemy.position;

        tRotation = Quaternion.LookRotation(direction);
        tRotation.z = -tRotation.y;
        tRotation.x = 0;
        tRotation.y = 0;

        Vector3 northDirection = new Vector3(0, 0, orientation.eulerAngles.y);
        indicators[value].localRotation = tRotation * Quaternion.Euler(northDirection);
        startTimes[value] = Time.time;
        value++;
        if (value >= indicators.Count)
        {
            value = 0;
        }
    }
}
