using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Demon", menuName = "Demon/Create Demon", order = 0)]
public class Demon : ScriptableObject
{
    public DemonId demon;
    public GameObject demonObject;
}
