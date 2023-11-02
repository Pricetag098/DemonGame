using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemonMaterials : MonoBehaviour
{
    private static Material[] defaultDemonMaterials;
    private static Material[] defaultClothMaterials;
    private static Material[] defaultAttachMaterials;

    private static Material[] Ritual1;
    private static Material[] Ritual2;
    private static Material[] Ritual3;
    private static Material[] Ritual4;

    private void Awake()
    {
        defaultDemonMaterials = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/DefaultMaterialVariations");
        defaultClothMaterials = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/ClothMaterialVariations");
        defaultAttachMaterials = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/AttachmentMaterialVariations");

        Ritual1 = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/1stRitualMaterialVariations");
        Ritual2 = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/2stRitualMaterialVariations");
        Ritual3 = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/3stRitualMaterialVariations");
        Ritual4 = LoadALlAssestsFromFolder<Material>("Materials/LesserDemonMaterialVarients/4stRitualMaterialVariations");

        Resources.UnloadUnusedAssets();
    }
    public static void SetClothMaterial(GameObject obj)
    {
        int num = Random.Range(0, defaultClothMaterials.Length);

        if(obj.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
        {
            Material[] mats = meshRenderer.materials;
            mats[0] = defaultClothMaterials[num];

            meshRenderer.materials = mats;
        }
    }

    public static void SetAttachmentMaterial(GameObject obj)
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

    public static void SetRitualMaterial(SkinnedMeshRenderer meshRenderer, int index)
    {
        switch(index)
        {
            case 1:
                int num1 = Random.Range(0, Ritual1.Length);

                Material[] mats1 = meshRenderer.materials;
                mats1[1] = Ritual1[num1];

                meshRenderer.materials = mats1;
                break;
            case 2:
                int num2 = Random.Range(0, Ritual2.Length);

                Material[] mats2 = meshRenderer.materials;
                mats2[1] = Ritual2[num2];

                meshRenderer.materials = mats2;
                break;
            case 3:
                int num3 = Random.Range(0, Ritual3.Length);

                Material[] mats3 = meshRenderer.materials;
                mats3[1] = Ritual3[num3];

                meshRenderer.materials = mats3;
                break;
            case 4:
                int num4 = Random.Range(0, Ritual4.Length);

                Material[] mats4 = meshRenderer.materials;
                mats4[1] = Ritual3[num4];

                meshRenderer.materials = mats4;
                break;
        }
    }

    public T[] LoadALlAssestsFromFolder<T>(string FilePath)
    {
        return Resources.LoadAll(FilePath).Cast<T>().ToArray();
    }
}
