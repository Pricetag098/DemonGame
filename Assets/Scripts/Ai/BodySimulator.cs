using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySimulator : MonoBehaviour
{
    [System.Serializable]
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
            //DestroyImmediate(target.GetComponent<Collider>());
            DestroyImmediate(target.GetComponent<CharacterJoint>());
            DestroyImmediate(target.GetComponent<Rigidbody>());
            transformData.Load(target);
            if(target.childCount > 0) target = target.GetChild(0);
        }
        Destroy(this);
    }
}
