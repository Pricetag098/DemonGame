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
    public int CurrentCells;

    [Header("Gizmos")]
    public bool ShowGizmos;

    public HashGrid3D<AiAgent> cells;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Instance.cells = new HashGrid3D<AiAgent>(cellSize);
            TotalCells = (int)cells.CellCount;
        }
        else
        {
            Destroy(gameObject);
            CreateNewHasHGrid(cellSize);
        }
    }

    public static void CreateNewHasHGrid(Vector3 cellSize)
    {
        Instance.cells = new HashGrid3D<AiAgent>(cellSize);
    }

    void Start()
    {
        //int count = DemonSpawner.ActiveDemons.Count;

        //for (int i = 0; i < count; i++)
        //{
        //    cells.Insert(DemonSpawner.ActiveDemons[i].GetComponent<SpatialHashObject>());
        //}
    }

    void Update()
    {
        //int count = DemonSpawner.ActiveDemons.Count;

        //for (int i = 0; i < count; i++)
        //{
        //    DemonFramework demon = DemonSpawner.ActiveDemons[i];

        //    if(demon.IsAlive() == true)
        //    {
        //        demon.UpdateAgentNearby(cells.UpdateObjectAndGetSurroudingObjects(demon.GetAgent));
        //    }
        //}

        //CurrentCells = cells.cellPositions.Count;
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

                    Gizmos.DrawWireCube(new Vector3(v.x * cellSize.x, v.y * cellSize.y, v.z * cellSize.z) + cellSize / 2, cellSize); 
                }
            }
        }
    }
}
