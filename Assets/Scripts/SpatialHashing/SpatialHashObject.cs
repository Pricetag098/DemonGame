using System.Collections.Generic;
using UnityEngine;

namespace BlakesSpatialHash
{
    public interface ISpatialHash3D
    {
        Vector3 GetPosition { get; }
        Vector3 GetLastPosition { get; set; }
        uint Index { get; set; }
        bool Enabled { get; }
        List<AiAgent> Objects { get; set; }
    }

    public class SpatialHashObject : MonoBehaviour, ISpatialHash3D
    {
        public Vector3 GetPosition { get { return transform.position; } }
        public Vector3 GetLastPosition { get; set; }
        public uint Index { get; set; }
        public bool Enabled { get { return gameObject.activeSelf; } }
        public List<AiAgent> Objects { get; set; }

        [HideInInspector] public SpatialHashGrid3D Grid;

        public void Initalise()
        {
            Grid = SpatialHashGrid3D.Instance;

            Objects = new List<AiAgent>();
        }
    }
}