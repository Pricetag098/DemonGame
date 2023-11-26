using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGun : MonoBehaviour
{
    public List<Material> materials;

    public List<Renderer> geo;
    public void ChangeMat(int tier)
    {
        foreach(Renderer child in geo)
        {
            child.material = materials[tier];
        }
    }
}
