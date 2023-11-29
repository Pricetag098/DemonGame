using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlsViewer : MonoBehaviour
{
    public List<GameObject> objectsToSwap = new List<GameObject>();
    private int currentIndex = 0;

    private void Start()
    {
        if (objectsToSwap.Count > 0)
        {
            SetActiveObject(currentIndex);
        }

    }

    public void OnLeftButtonClick()
    {
        CycleObjects(-1); 
    }

    public void OnRightButtonClick()
    {
        CycleObjects(1);
    }

    private void CycleObjects(int direction)
    {
        currentIndex = (currentIndex + direction + objectsToSwap.Count) % objectsToSwap.Count;
        SetActiveObject(currentIndex);
    }

    private void SetActiveObject(int index)
    {
        foreach (GameObject obj in objectsToSwap)
        {
            obj.SetActive(false);
        }
        objectsToSwap[index].SetActive(true);
    }
}
