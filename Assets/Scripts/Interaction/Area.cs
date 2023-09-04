using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public Areas AreaId;
    [SerializeField] bool discovered;
    public int baseDepth;
    public int specialDepth;
    [HideInInspector] public Vector3 position;

    public List<Spawner> baseSpawns;
    public List<Spawner> specialSpawns;
    
    public List<Area> AdjacentAreas = new List<Area>();

    private void Awake()
    {
        position = transform.position;
    }
}
