using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public Areas AreaId;
    public bool discovered;
    public int baseDepth;
    public int specialDepth;
    [HideInInspector] public Vector3 position;

    public List<Spawner> baseSpawns;
    public List<Spawner> specialSpawns;
    
    // swap out adjacent areas to optionals 
    public List<Optional<AreaConnect>> OptionalAreas = new List<Optional<AreaConnect>>();

    private void Awake()
    {
        position = transform.position;
    }
}

[System.Serializable]
public class AreaConnect
{
    public Area Area;
    public bool Open;
}
