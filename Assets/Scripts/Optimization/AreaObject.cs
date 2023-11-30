using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaObject : MonoBehaviour
{
    public Areas ObjectArea;
    public AreaPriority ObjectPriority;
}

public enum AreaPriority
{
    Null,
    LOW_PRIORITY,
    HIGH_PRIORITY,
    ENVIRONMENT
}