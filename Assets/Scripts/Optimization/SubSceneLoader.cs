using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SubSceneLoader : MonoBehaviour
{
    public static Areas CurrentArea { get { return DetectArea.CurrentArea; } }
    private static Areas LastArea { get; set; }

    public delegate void Action();
    public static Action AreaUpdate;

    private void Awake()
    {
        LastArea = Areas.Null;
    }

    private void Update()
    {
        UpdateSubScenes();
    }

    public static bool UpdateSubScenes()
    {
        if(LastArea != CurrentArea)
        {
            LastArea = CurrentArea;

            if(AreaUpdate != null)
            {
                AreaUpdate();
            }
            return true;
        }

        return false;
    }
}
