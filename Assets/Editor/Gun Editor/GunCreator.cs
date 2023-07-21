using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GunCreator : EditorWindow
{
    [SerializeField] public GameObject baseGun;

    GameObject currentGun;

    GameObject gunModel;

    GameObject vfx;

    bool hasVFX;

    bool hasVisualiser;

    String gunName;

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

        GUILayout.Space(5);

        gunName = EditorGUILayout.TextField("Gun Name", gunName, GUILayout.Width(455));
        EditorGUILayout.LabelField("Name of the Gun.", EditorStyles.miniLabel);

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

        gun.shotsPerFiring = EditorGUILayout.IntField("Shots Per Firing", gun.shotsPerFiring, GUILayout.Width(455));
        EditorGUILayout.LabelField("Amount of bullets for every fire input (Primarily for shotguns).", EditorStyles.miniLabel);

        GUILayout.Space(5);

        gun.maxAmmo = EditorGUILayout.IntField("Clip Size", gun.maxAmmo, GUILayout.Width(455));
        EditorGUILayout.LabelField("Amount of bullets in each magazine.", EditorStyles.miniLabel);

        GUILayout.Space(5);

        gun.roundsPerMin = EditorGUILayout.FloatField("Rounds per minute", gun.roundsPerMin, GUILayout.Width(455));
        EditorGUILayout.LabelField("Amount of bullets fired in a minute (Doesn't include increased Shots Per Firing).", EditorStyles.miniLabel);

        GUILayout.Space(5);

        gun.reloadDuration = EditorGUILayout.FloatField("Reload Duration", gun.reloadDuration, GUILayout.Width(455));
        EditorGUILayout.LabelField("Amount of seconds to complete a reload.", EditorStyles.miniLabel);

        if (gun.fireSelect == Gun.FireTypes.burst)
        {
            GUILayout.Space(5);

            gun.burstRounds = EditorGUILayout.IntField("Shots Per Burst", gun.burstRounds, GUILayout.Width(455));
            EditorGUILayout.LabelField("Amount of bullets fired per bust (Doesn't include increased Shots Per Firing).", EditorStyles.miniLabel);

            GUILayout.Space(5);

            gun.burstRounds = EditorGUILayout.IntField("Shots Per Burst", gun.burstRounds, GUILayout.Width(455));
            EditorGUILayout.LabelField("Amount of bullets fired per bust (Doesn't include increased Shots Per Firing).", EditorStyles.miniLabel);
        }

        GUILayout.Space(15);
        EditorGUILayout.LabelField("Stash Settings", EditorStyles.boldLabel);
        GUILayout.Space(5);

        gun.stash = EditorGUILayout.IntField("Stash", gun.stash, GUILayout.Width(455));
        EditorGUILayout.LabelField("The amount of spare ammo the gun will start with.", EditorStyles.miniLabel);

        gun.maxStash = EditorGUILayout.IntField("Max Stash", gun.maxStash, GUILayout.Width(455));
        EditorGUILayout.LabelField("The maximum spare ammo a player can carry for this gun.", EditorStyles.miniLabel);

        GUILayout.Space(15);
        EditorGUILayout.LabelField("Penetration Settings", EditorStyles.boldLabel);

        GUILayout.Space(5);

        gun.maxPenetrations = EditorGUILayout.IntField("Max Penetrations", gun.maxPenetrations, GUILayout.Width(455));
        EditorGUILayout.LabelField("Amount of enemies that a bullet can penetrate through.", EditorStyles.miniLabel);

        GUILayout.Space(5);

        gun.validPenetrations = EditorGUILayout.LayerField("Valid Penetrations", gun.validPenetrations, GUILayout.Width(455));
        EditorGUILayout.LabelField("Layers a bullet can penetrate through.", EditorStyles.miniLabel);

        GUILayout.Space(5);

        gun.damageLossDivisor = EditorGUILayout.FloatField("Damage Loss Divisor", gun.damageLossDivisor, GUILayout.Width(455));
        EditorGUILayout.LabelField("Damage divided by x amount is the damage after each penetration.", EditorStyles.miniLabel);

        GUILayout.Space(15);
        EditorGUILayout.LabelField("Pre Fabs", EditorStyles.boldLabel);
        GUILayout.Space(5);

        gunModel = (GameObject)EditorGUILayout.ObjectField("Gun Model", gunModel, typeof(GameObject), allowSceneObjects: false, GUILayout.Width(305));
        EditorGUILayout.LabelField("The model of the gun for the prefab to use.", EditorStyles.miniLabel);

        GUILayout.Space(5);

        hasVFX = EditorGUILayout.Toggle("Has Muzzle Flash", hasVFX, GUILayout.Width(455));
        EditorGUILayout.LabelField("Muzzle flash VFX will play on shot.", EditorStyles.miniLabel);
        gun.gunfire.Enabled = hasVFX;

        if (hasVFX)
        {
            GUILayout.Space(5);

            vfx = (GameObject)EditorGUILayout.ObjectField("Gunfire", vfx, typeof(GameObject), allowSceneObjects: false, GUILayout.Width(305));
            EditorGUILayout.LabelField("The visual effects used for when firing.", EditorStyles.miniLabel);
        }

        GUILayout.Space(15);
        EditorGUILayout.LabelField("Bullet Visualiser", EditorStyles.boldLabel);

        GUILayout.Space(5);

        hasVisualiser = EditorGUILayout.Toggle("Has Bullet Visualiser", hasVisualiser, GUILayout.Width(455));
        EditorGUILayout.LabelField("Will a fake bullet be shot from the gun.", EditorStyles.miniLabel);
        gun.visualiserPool.Enabled = hasVisualiser;

        if (hasVisualiser)
        {
            GUILayout.Space(5);

            gun.bulletVisualiserSpeed = EditorGUILayout.FloatField("Visualiser Speed", gun.bulletVisualiserSpeed, GUILayout.Width(455));
            EditorGUILayout.LabelField("Speed of the fake bullet being shot.", EditorStyles.miniLabel);
        }


        GUILayout.Space(15);

        if (GUILayout.Button("Generate Gun", GUILayout.Width(150), GUILayout.Height(20)))
        {
            Instantiate(gunModel, currentGun.transform);

            if (hasVFX)
            {
                vfx = Instantiate(vfx, currentGun.transform);

                gun.gunfire.Value = vfx.GetComponent<ParticleSystem>();
            }

            string newWeaponPath = "Assets/Prefabs/Guns/" + gunName + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(currentGun, newWeaponPath);

            EditorWindow.GetWindow<GunCreator>("Gun Creator").Close();
        }

        GUILayout.Space(25);
        EditorGUILayout.EndScrollView();
    }

    private void OnDestroy()
    {
        DestroyImmediate(currentGun);
    }
}