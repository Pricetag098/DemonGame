using System.Collections;
using System.Collections.Generic;
using Unity.Scenes;
using UnityEngine;

public class SubSceneReferences : MonoBehaviour
{
    public static SubSceneReferences Instance { get; private set; }

    // ONE OF THESE FOR EACH AREA OF THE MAP
    public SubScene MainGate;
    public SubScene Courtyard;
    public SubScene Graveyard;
    public SubScene Garden;
    //public SubScene Kitchen;
    //public SubScene Library;
    //public SubScene BishopsQuarters;
    //public SubScene CathedralHallUpper;
    //public SubScene CathedralHallLower;
    //public SubScene Tomb;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
