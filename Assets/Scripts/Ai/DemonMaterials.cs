using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemonMaterials : MonoBehaviour
{
    private static Material[] defaultDemonMaterials;
    private static Material[] defaultClothMaterials;
    private static Material[] defaultAttachMaterials;

    private static Material[] Ritual;
    private static Material[] RitualClothMaterials;
    private static Material[] RitualAttachMaterials;

    private void Awake()
    {
        defaultDemonMaterials = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/DefaultMaterialVariations");
        defaultClothMaterials = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/ClothMaterialVariations");
        defaultAttachMaterials = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/AttachmentMaterialVariations");

        Ritual = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/RitualMaterialVariations");
        RitualClothMaterials = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/RitualMaterialVariations");
        RitualAttachMaterials = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/RitualMaterialVariations");

        Resources.UnloadUnusedAssets();
    }

    public static void SetDefaultClothMaterial(GameObject obj)
    {
        int num = Random.Range(0, defaultClothMaterials.Length);

        if(obj.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
        {
            Material[] mats = meshRenderer.materials;
            mats[0] = defaultClothMaterials[num];

            meshRenderer.materials = mats;
        }
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
        int num = Random.Range(0, defaultDemonMaterials.Length);

        Material[] mats = meshRenderer.materials;

        mats[1] = defaultDemonMaterials[num]; // sets second material as first is not the demon material

        meshRenderer.materials = mats;
    }

    public static void SetRitualMaterial(SkinnedMeshRenderer meshRenderer)
    {
        int num = Random.Range(0, Ritual.Length);

        Material[] mats = meshRenderer.materials;

        mats[1] = Ritual[num]; // sets second material as first is not the demon material

        meshRenderer.materials = mats;
    }

    public T[] LoadALlAssestsFromFolder<T>(string FilePath)
    {
        return Resources.LoadAll(FilePath).Cast<T>().ToArray();
    }
}
