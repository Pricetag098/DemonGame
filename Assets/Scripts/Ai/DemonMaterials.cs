//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemonMaterials : MonoBehaviour
{
    private static Material[] defaultDemonMaterials;
    private static Material[] defaultClothMaterials;
    private static Material[] defaultAttachMaterials;


    private static Material[] chaosMaterials;

    private static Material[] Ritual;
    private static Material[] RitualClothMaterials;
    private static Material[] RitualAttachtmentMaterials;

    private void Awake()
    {
        defaultDemonMaterials = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/DefaultMaterialVariations");
        defaultClothMaterials = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/ClothMaterialVariations");
        defaultAttachMaterials = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/AttachmentMaterialVariations");

        Ritual = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/RitualMaterialVariations");
        RitualClothMaterials = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/RitualClothMaterialVariations");
        RitualAttachtmentMaterials = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/RitualAttatchmentmaterialVariations");

        chaosMaterials = LoadALlAssestsFromFolder<Material>("Materials/ChaosDemonMaterialVarients/DefaultMaterialVariations");

        Resources.UnloadUnusedAssets();
    }

    public static void SetDefaultAttachmentMaterial(GameObject obj)
    {
        int num = Random.Range(0, defaultAttachMaterials.Length);

        if (obj.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
        {
            Material[] mats = meshRenderer.materials;
            mats[0] = defaultAttachMaterials[num];

            meshRenderer.materials = mats;
        }
    }

    public static void SetDefaultSpawningMaterial(SkinnedMeshRenderer meshRenderer)
    {
        Material[] mats = meshRenderer.materials;

        int num = Random.Range(0, defaultClothMaterials.Length);

        mats[0] = defaultClothMaterials[num]; // sets cloth

        num = Random.Range(0, defaultDemonMaterials.Length);

        mats[1] = defaultDemonMaterials[num]; // sets demon

        meshRenderer.materials = mats;
    }

    public static void SetRitualMaterial(SkinnedMeshRenderer meshRenderer)
    {
        Material[] mats = meshRenderer.materials;

        int num = Random.Range(0, RitualClothMaterials.Length);

        mats[0] = RitualClothMaterials[num]; // sets cloth

        num = Random.Range(0, Ritual.Length);

        mats[1] = Ritual[num]; // sets demon

        meshRenderer.materials = mats;
    }

    public static void SetRitualAttachmentMaterial(GameObject obj)
    {
        int num = Random.Range(0, RitualAttachtmentMaterials.Length);

        if (obj.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
        {
            Material[] mats = meshRenderer.materials;
            mats[0] = RitualAttachtmentMaterials[num];

            Debug.Log(RitualAttachtmentMaterials[num].name);

            meshRenderer.materials = mats;
        }
    }

    public static void SetChaosMaterial(SkinnedMeshRenderer meshRenderer)
    {
        Material[] mats = meshRenderer.materials;

        int num = Random.Range(0, chaosMaterials.Length);

        mats[0] = chaosMaterials[num]; // sets choas material

        meshRenderer.materials = mats;
    }

    public T[] LoadALlAssestsFromFolder<T>(string FilePath)
    {
        return Resources.LoadAll(FilePath).Cast<T>().ToArray();
    }
}
