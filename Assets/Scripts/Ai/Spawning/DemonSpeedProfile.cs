using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonInfo
{
    [System.Serializable]
    public class DemonSpeedProfile
    {
        [HideInInspector] public float Speed = 0;
        public float minSpeed = 0;
        public float maxSpeed = 0;

        public float GetSpeed()
        {
            return Speed = Random.Range(minSpeed, maxSpeed);
        }
    }
    public enum SpeedType
    {
        Null,
        Walker,
        Jogger,
        Runner
    }
}
