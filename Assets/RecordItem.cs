using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordItem : MonoBehaviour
{
    RecordManager recordManager;

    public AudioClip song;

    public GameObject model;

    public int discNum;

    private void Awake()
    {
        recordManager = FindObjectOfType<RecordManager>();
    }

    private void Start()
    {
        model.SetActive(false);
    }

    public void PickupItem()
    {
        if (recordManager.currentRecord != null)
        {
            recordManager.currentRecord.ReplaceItem();
        }
        recordManager.currentRecord = this;
        model.SetActive(false);
        recordManager.HasDisc();
    }

    public void ReplaceItem()
    {
        model.SetActive(true);
    }

}
