using System.Collections;
using System.Collections.Generic;
using Unity.Scenes;
using UnityEngine;

public class SubSceneReferences : MonoBehaviour
{
    public static SubSceneReferences Instance { get; private set; }

    public SubScene scene1;

    private void Awake()
    {
        Instance = this;
    }
}
