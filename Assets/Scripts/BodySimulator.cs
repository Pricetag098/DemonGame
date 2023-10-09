using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySimulator : MonoBehaviour
{
    public struct TransformData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public TransformData(Transform t)
        {
            Position = t.localPosition;
            Rotation = t.localRotation;
        }
        public void Load(Transform t)
        {
            t.localPosition = Position;
            t.localRotation = Rotation;
        }
    }

    [SerializeField] List<TransformData> transformDatas = new List<TransformData>();
    [ContextMenu("Save")]
    void Save()
    {
        Transform target = transform;
        transformDatas.Add(new TransformData(target));
        while (target.childCount > 0)
        {
            target = target.GetChild(0);
            transformDatas.Add(new TransformData(target));
            
        }
    }
    [ContextMenu("Load")]
    void Load()
    {
        Transform target = transform;
        foreach (TransformData transformData in transformDatas)
        {
            transformData.Load(target);
            target = target.GetChild(0);
        }
    }
}
