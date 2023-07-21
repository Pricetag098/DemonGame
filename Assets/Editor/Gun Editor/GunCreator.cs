using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GunCreator : EditorWindow
{
    [SerializeField] public GameObject baseGun;

    GameObject currentGun;

    Gun gun;

    Vector2 scrollPos;

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
        currentGun = PrefabUtility.InstantiatePrefab(baseGun) as GameObject;
        gun = currentGun.GetComponent<Gun>();
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(position.height));

        EditorGUILayout.LabelField("Gun Settings", EditorStyles.boldLabel);
        GUILayout.Space(10);

        gun.fireSelect = (Gun.FireTypes)EditorGUILayout.EnumPopup("Fire Type", gun.fireSelect, GUILayout.Width(455));
        EditorGUILayout.LabelField("The Fire Type e.g. Full Auto, Burst.", EditorStyles.miniLabel);

        GUILayout.Space(5);

        gun.damage = EditorGUILayout.FloatField("Damage", gun.damage, GUILayout.Width(455));
        EditorGUILayout.LabelField("The base a damage a shot deals.", EditorStyles.miniLabel);

        GUILayout.Space(5);

        gun.bulletRange = EditorGUILayout.FloatField("Bullet Range", gun.bulletRange, GUILayout.Width(455));
        EditorGUILayout.LabelField("Max range the bullet can register a hit.", EditorStyles.miniLabel);

        GUILayout.Space(5);

        gun.bulletSpreadDegrees = EditorGUILayout.FloatField("Bullet Spread", gun.bulletSpreadDegrees, GUILayout.Width(455));
        EditorGUILayout.LabelField("The max bullet spread in degrees.", EditorStyles.miniLabel);

        GUILayout.Space(5);

        gun.bulletSpreadDegrees = EditorGUILayout.FloatField("Bullet Spread", gun.bulletSpreadDegrees, GUILayout.Width(455));
        EditorGUILayout.LabelField("The max bullet spread in degrees.", EditorStyles.miniLabel);




        GUILayout.Space(75);
        EditorGUILayout.EndScrollView();
    }

    private void OnDestroy()
    {
        DestroyImmediate(currentGun);
    }
}