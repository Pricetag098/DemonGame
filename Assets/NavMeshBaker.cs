using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[ExecuteInEditMode]
public class NavMeshBaker : MonoBehaviour
{
    public List<NavMeshSurface> navmesh;
    public List<MeshRenderer> meshRenderers;
    public List<GameObject> objects;

    public void TurnOffObjects()
    {
        foreach (MeshRenderer mesh in meshRenderers)
        {
            mesh.enabled = true;
        }

        foreach (GameObject gameObject in objects)
        {
            gameObject.SetActive(false);
        }
    }

    public void TurnOnObjects()
    {
        foreach (MeshRenderer mesh in meshRenderers)
        {
            mesh.enabled = false;
        }

        foreach (GameObject gameObject in objects)
        {
            gameObject.SetActive(true);
        }
    }

    public void Bake()
    {
        foreach (NavMeshSurface surface in navmesh)
        {
            surface.BuildNavMesh();
        }
    }
}
