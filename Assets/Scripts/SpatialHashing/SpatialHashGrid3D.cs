using System.Collections.Generic;
using UnityEngine;
using BlakesSpatialHash;
using System.Linq;

public class SpatialHashGrid3D : MonoBehaviour
{
    public static SpatialHashGrid3D Instance;

    [Header("Cell Size")]
    public Vector3 cellSize;

    [Header("Total Cells")]
    public int TotalCells;

    [Header("Gizmos")]
    public bool ShowGizmos;

    public HashGrid3D<SpatialHashObject> cells;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Instance.cells = new HashGrid3D<SpatialHashObject>(cellSize);
            TotalCells = (int)cells.CellCountMax;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        int count = DemonSpawner.ActiveDemons.Count;

        Debug.Log(count);

        for (int i = 0; i < count; i++)
        {
            cells.Insert(DemonSpawner.ActiveDemons[i].GetComponent<SpatialHashObject>());
        }
    }

    void Update()
    {
        int count = DemonSpawner.ActiveDemons.Count;

        for (int i = 0; i < count; i++)
        {
            DemonBase temp = DemonSpawner.ActiveDemons[i];

            if(temp.CanUpdateSpatialIndex())
            {
                temp.UpdateAgentNearby(cells.UpdateObjectAndGetSurroudingObjects(temp.GetSpatialHashObject()));
            }
        }

        Debug.Log(cells.cellPositions.Count);
    }

    public void OnDrawGizmos()
    {
        if(ShowGizmos == true)
        {
            Gizmos.color = Color.magenta;
            if (cells != null)
            {
                int count = cells.cellPositions.Count;

                for (int i = 0; i < count; i++)
                {
                    Vector3 v = cells.cellPositions[i];

                    Gizmos.DrawWireCube(new Vector3(v.x * cellSize.x, v.y * cellSize.y, v.z * cellSize.z) +
                                       new Vector3(cellSize.x / 2, cellSize.y / 2, cellSize.z / 2), cellSize);
                }
            }
        }
    }
}
