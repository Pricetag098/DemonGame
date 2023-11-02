using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SubSceneLoader : MonoBehaviour
{
    private SubSceneReferences refs;

    // get current area from detectarea
    public static Areas CurrentArea { get { return DetectArea.CurrentArea; } }
    private static Areas LastArea { get; set; }

    // break each area up into smaller chunks and load based off player position
    // grab the current area and where the player is facing to load / unload objects as the player moves around

    private void Awake()
    {
        LastArea = Areas.Null;
    }

    public void Start()
    {
        refs = SubSceneReferences.Instance;
    }

    public void Update()
    {
        
    }

    public static bool UpdateSubScenes()
    {
        if(LastArea != CurrentArea)
        {
            LastArea = CurrentArea;
            return true;
        }

        return false;
    }
}
