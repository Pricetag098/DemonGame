using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NavMeshBaker))]
public class CustomInspectorButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NavMeshBaker navBaker = (NavMeshBaker)target;

        if (GUILayout.Button("TurnOff"))
        {
            navBaker.TurnOffObjects();
        }

        if (GUILayout.Button("TurnOn"))
        {
            navBaker.TurnOnObjects();
        }

        if (GUILayout.Button("Bake"))
        {
            navBaker.Bake();
        }
    }
}
