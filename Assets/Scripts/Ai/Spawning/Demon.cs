using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonCum;

[CreateAssetMenu(fileName = "Demon", menuName = "Demon/Create Demon", order = 0)]
public class Demon : ScriptableObject
{
    public DemonID demon;
    public GameObject demonObject;
}
