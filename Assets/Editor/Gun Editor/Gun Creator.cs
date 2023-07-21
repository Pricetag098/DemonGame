using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GunCreator : EditorWindow
{
    [SerializeField] GameObject baseGun;


    [MenuItem("Window/Gun Creator")]
    public static void OpenWindow()
    {
        GunCreator window = EditorWindow.GetWindow<GunCreator>("Gun Creator");
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
}