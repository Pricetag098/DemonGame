using System.Collections;
using System.Collections.Generic;
using Unity.Scenes;
using UnityEngine;

public class SubSceneReferences : MonoBehaviour
{
    public static SubSceneReferences Instance { get; private set; }

    // ONE OF THESE FOR EACH AREA OF THE MAP
    public SubScene MainGate;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
