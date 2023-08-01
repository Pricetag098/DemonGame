using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonCum;

/// <summary>
/// percentage based spawning
/// </summary>

[CreateAssetMenu(fileName = "Wave", menuName = "Wave/Create Wave", order = 0)]
public class Wave : ScriptableObject
{
    public List<DemonType> types = new List<DemonType>();
}