using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VFXGen : EditorWindow
{
    [MenuItem("Window/VFX Generator")]
    public static void OpenWindow()
    {
        VFXGen window = EditorWindow.GetWindow<VFXGen>("VFX Generator");
        window.minSize = new Vector2(475f, 500f);
        window.maxSize = new Vector2(475f, 800f);
        window.Show();
    }

    private void CreateGUI()
    {

    }

    private void OnGUI()
    {

    }

    private void OnDestroy()
    {

    }

}
