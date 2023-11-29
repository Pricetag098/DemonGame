using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLoaderGameObject : MonoBehaviour
{
    private static Dictionary<Areas, Dictionary<AreaPriority, AreaPriorityObjects>> AreaObjects;

    #region Area
    Areas CourtYard = Areas.Courtyard;
    Areas Graveyard = Areas.Graveyard;
    Areas MainEntrance = Areas.MainEntrance;
    Areas Garden = Areas.Garden;
    Areas Kitchen = Areas.Kitchen;
    Areas Library = Areas.Library;
    Areas BishopsQuaters = Areas.BishopsQuarters;
    Areas CathedralHallUpper = Areas.CathedralHallUpper;
    Areas CathedralHallLower = Areas.CathedralHallLower;
    Areas Tomb = Areas.Tomb;

    private AreaPriority LowPriority = AreaPriority.LOW_PRIORITY;
    private AreaPriority HighPriority = AreaPriority.HIGH_PRIORITY;
    #endregion

    private void Awake()
    {
        CreateDictionary();

        SubSceneLoader.AreaUpdate += OnUpdateArea;
    }

    public void OnUpdateArea()
    {
        switch (SubSceneLoader.CurrentArea)
        {
            case Areas.MainEntrance:
                // Areas To Load
                LoadAll(CourtYard);
                LoadAll(Garden);
                LoadAreaObjects(Graveyard, HighPriority);

                // Areas To Unload
                UnloadAreaObjects(Graveyard, LowPriority);
                UnloadAreaObjects(CathedralHallLower, LowPriority);
                break;
            case Areas.Garden:
                // Areas To Load
                LoadAll(MainEntrance);
                LoadAreaObjects(CourtYard, HighPriority);
                LoadAll(CathedralHallLower);

                // Areas To Unload
                UnloadAll(Graveyard);
                UnloadAreaObjects(CourtYard, LowPriority);
                UnloadAll(Library);
                break;
            case Areas.Courtyard:
                // Areas To Load
                LoadAll(Graveyard);
                LoadAll(MainEntrance);
                LoadAreaObjects(Garden, HighPriority);
                LoadAll(CathedralHallLower);

                // Areas To Unload
                UnloadAll(Library);
                UnloadAreaObjects(Garden, LowPriority);
                break;
            case Areas.Graveyard:
                // Areas To Load
                LoadAll(Library);
                LoadAll(CourtYard);
                LoadAreaObjects(MainEntrance, HighPriority);

                // Areas To Unload
                UnloadAreaObjects(MainEntrance, LowPriority);
                UnloadAll(Garden);
                UnloadAreaObjects(CathedralHallLower, LowPriority);
                break;
            case Areas.Tomb:
                // Areas To Load
                LoadAll(Graveyard);
                LoadAll(MainEntrance);
                LoadAreaObjects(Garden, HighPriority);
                LoadAreaObjects(CathedralHallLower, HighPriority);
                LoadAreaObjects(Library, HighPriority);

                // Areas To Unload
                UnloadAreaObjects(CathedralHallLower, LowPriority);
                UnloadAreaObjects(Garden, LowPriority);
                UnloadAll(Library);
                break;
        }
    }

    #region DICTIONARY CREATION
    private void InitaliseDictionary()
    {
        AddArea(Areas.Courtyard);
        AddArea(Areas.Graveyard);
        AddArea(Areas.MainEntrance);
        AddArea(Areas.Garden);
        AddArea(Areas.Kitchen);
        AddArea(Areas.Library);
        AddArea(Areas.BishopsQuarters);
        AddArea(Areas.CathedralHallUpper);
        AddArea(Areas.CathedralHallLower);
        AddArea(Areas.Tomb);
    }

    private void AddArea(Areas area)
    {
        AreaObjects.Add(area, new Dictionary<AreaPriority, AreaPriorityObjects>());
        AreaObjects[area].Add(AreaPriority.LOW_PRIORITY, new AreaPriorityObjects());
        AreaObjects[area].Add(AreaPriority.HIGH_PRIORITY, new AreaPriorityObjects());
    }

    public void CreateDictionary()
    {
        AreaObjects = new Dictionary<Areas, Dictionary<AreaPriority, AreaPriorityObjects>>();

        InitaliseDictionary();

        AreaObject[] objs = FindObjectsOfType<AreaObject>();

        foreach (AreaObject obj in objs) 
        {
            AreaObjects[obj.ObjectArea][obj.ObjectPriority].AreaObjects.Add(obj.transform.gameObject);
        }
    }
    #endregion

    #region DICTIONARY ACCESSORS
    private void LoadAll(Areas area)
    {
        LoadAreaObjects(area, LowPriority);
        LoadAreaObjects(area, HighPriority);
    }

    private void UnloadAll(Areas area)
    {
        UnloadAreaObjects(area, LowPriority);
        UnloadAreaObjects(area, HighPriority);
    }
    private void LoadAreaObjects(Areas area, AreaPriority priority)
    {
        if(IsAreaActive(area, priority) == true) { return; }

        AreaObjects[area][priority].active = true;

        foreach(GameObject g in AreaObjects[area][priority].AreaObjects)
        {
            g.SetActive(true);
        }
    }
    private void UnloadAreaObjects(Areas area, AreaPriority priority)
    {
        if (IsAreaActive(area, priority) == false) { return; }

        AreaObjects[area][priority].active = false;

        foreach (GameObject g in AreaObjects[area][priority].AreaObjects)
        {
            g.SetActive(false);
        }
    }

    private bool IsAreaActive(Areas area, AreaPriority priority)
    {
        return AreaObjects[area][priority].active;
    }
    #endregion

    public class AreaPriorityObjects
    {
        public List<GameObject> AreaObjects { get; private set; }
        public bool active { get; set; }

        public AreaPriorityObjects()
        {
            AreaObjects = new List<GameObject>();
            active = false;
        }
    }
}
