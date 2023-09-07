using Codice.Client.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VFXGen : EditorWindow
{
    const string BasePrefabPath = "VFX/BaseVfx";

    GameObject VFXBase;

    GameObject VFXObject;

    GameObject VFXToSpawn;

    GameObject SpawnedVFX;

    GameObject SavedVFX;

    bool generatedVFX;

    bool finished;

    VfxSpawnRequest SpawnRequest;

    SoundPlayer SoundPlayer;

    string VFXName;

    Vector2 scrollPos;


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
        VFXBase = Resources.Load<GameObject>(BasePrefabPath);

        SpawnRequest = ScriptableObject.CreateInstance<VfxSpawnRequest>();
        VFXObject = PrefabUtility.InstantiatePrefab(VFXBase) as GameObject;

        SoundPlayer = VFXObject.GetComponentInChildren<SoundPlayer>();
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(position.height));
        if (!generatedVFX && !finished)
        {
            EditorGUILayout.LabelField("VFX", EditorStyles.boldLabel);
            GUILayout.Space(10);

            VFXName = EditorGUILayout.TextField("VFX Name", VFXName, GUILayout.Width(455));
            EditorGUILayout.LabelField("This will be the name of the file.", EditorStyles.miniLabel);

            GUILayout.Space(5);

            VFXToSpawn = (GameObject)EditorGUILayout.ObjectField("VFX", VFXToSpawn, typeof(GameObject), allowSceneObjects: false, GUILayout.Width(305));
            EditorGUILayout.LabelField("VFX for the spawner to use.", EditorStyles.miniLabel);

            GUILayout.Space(5);

            SpawnRequest.poolSize = EditorGUILayout.IntField("Pool Size", SpawnRequest.poolSize, GUILayout.Width(455));
            EditorGUILayout.LabelField("Size of the VFX pool.", EditorStyles.miniLabel);

            GUILayout.Space(10);

            EditorGUILayout.LabelField("Audio", EditorStyles.boldLabel);
            for (int i = 0; i < SoundPlayer.clips.Count; i++)
            {
                SoundPlayer.clips[i] = (AudioClip)EditorGUILayout.ObjectField("VFX Audio Clip " + i.ToString(), SoundPlayer.clips[i], typeof(AudioClip), allowSceneObjects: false, GUILayout.Width(305));
            }
            GUILayout.BeginHorizontal();
            GUILayout.Space(355);
            if (GUILayout.Button("+", GUILayout.Width(50), GUILayout.Height(15)))
            {
                SoundPlayer.clips.Add(null);
            }
            if (GUILayout.Button("-", GUILayout.Width(50), GUILayout.Height(15)))
            {
                SoundPlayer.clips.Remove(SoundPlayer.clips[SoundPlayer.clips.Count - 1]);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(2f);
            EditorGUILayout.LabelField("Clips that are played when shooting.", EditorStyles.miniLabel);

            GUILayout.Space(5);

            SoundPlayer.pitchRange = EditorGUILayout.FloatField("Pitch Range", SoundPlayer.pitchRange, GUILayout.Width(455));
            EditorGUILayout.LabelField("Pitch range of the clips when played.", EditorStyles.miniLabel);

            GUILayout.Space(5);

            SoundPlayer.basePitch = EditorGUILayout.FloatField("Base Pitch", SoundPlayer.basePitch, GUILayout.Width(455));
            EditorGUILayout.LabelField("Base pitch of the clips when played.", EditorStyles.miniLabel);

            GUILayout.Space(10);

            if (GUILayout.Button("Generate VFX", GUILayout.Width(150), GUILayout.Height(20)))
            {
                if(VFXToSpawn != null)
                {
                    SpawnedVFX = Instantiate(VFXToSpawn, VFXObject.transform);
                }

                generatedVFX = true;
            }
        }
        else if (generatedVFX && !finished)
        {
            if(VFXToSpawn != null)
            {
                if (GUILayout.Button("Flip VFX Up", GUILayout.Width(150), GUILayout.Height(20)))
                {
                    SpawnedVFX.transform.forward = SpawnedVFX.transform.up;
                }
                EditorGUILayout.LabelField("Flips the forward Vector of the VFX to face up.", EditorStyles.miniLabel);

                GUILayout.Space(10);
            }


            if (GUILayout.Button("Finish and Save", GUILayout.Width(150), GUILayout.Height(20)))
            {
                string prefabPath = "Assets/Prefabs/VFX/" + VFXName + ".prefab";

                SavedVFX = PrefabUtility.SaveAsPrefabAsset(VFXObject, prefabPath);

                SpawnRequest.prefab = SavedVFX;

                string requestLocation = "Assets/Resources/VFX/" + VFXName + ".asset";

                AssetDatabase.CreateAsset(SpawnRequest, requestLocation);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                finished = true;
            }
        }
        else
        {
            EditorGUILayout.LabelField("Your vfx is saved at Resources > VFX > " + VFXName + ".", EditorStyles.boldLabel);

            GUILayout.Space(5);

            if (GUILayout.Button("Close Window", GUILayout.Width(150), GUILayout.Height(20)))
            {
                EditorWindow.GetWindow<VFXGen>("VFX Generator").Close();
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void OnDestroy()
    {
        DestroyImmediate(VFXObject);
    }

}
