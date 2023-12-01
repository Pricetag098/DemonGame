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
    Areas LibraryLower = Areas.LibraryLower;
    Areas BishopsQuaters = Areas.BishopsQuarters;
    Areas CathedralHallUpper = Areas.CathedralHallUpper;
    Areas CathedralHallLower = Areas.CathedralHallLower;
    Areas Tomb = Areas.Tomb;
    Areas LibraryUpper = Areas.LibaryUpper;

    private AreaPriority LowPriority = AreaPriority.LOW_PRIORITY;
    private AreaPriority HighPriority = AreaPriority.HIGH_PRIORITY;
    private AreaPriority Environment = AreaPriority.ENVIRONMENT;
    #endregion

    private void Awake()
    {
        CreateDictionary();

        LoadAllAreas();

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
                LoadAreaObjects(Graveyard, Environment);
                LoadAll(Tomb);

                // Areas To Unload
                UnloadAreaObjects(Graveyard, LowPriority);
                UnloadAreaObjects(Graveyard, Environment);
                UnloadAreaObjects(CathedralHallLower, LowPriority);
                UnloadAll(LibraryLower);
                UnloadAreaObjects(LibraryLower, Environment);
                UnloadAll(LibraryUpper);
                UnloadAreaObjects(LibraryUpper, Environment);
                break;
            case Areas.Garden:
                // Areas To Load
                LoadAll(MainEntrance);
                LoadAreaObjects(CourtYard, HighPriority);
                LoadAreaObjects(CourtYard, Environment);
                LoadAll(CathedralHallLower);

                // Areas To Unload
                UnloadAll(Graveyard);
                UnloadAreaObjects(Graveyard, Environment);
                UnloadAreaObjects(CourtYard, LowPriority);
                UnloadAll(LibraryLower);
                UnloadAreaObjects(LibraryLower, Environment);
                UnloadAll(LibraryUpper);
                UnloadAreaObjects(LibraryUpper, Environment);
                UnloadAll(Tomb);
                break;
            case Areas.Courtyard:
                // Areas To Load
                LoadAll(Graveyard);
                LoadAll(MainEntrance);
                LoadAreaObjects(Garden, HighPriority);
                LoadAreaObjects(Garden, Environment);
                LoadAll(CathedralHallLower);
                LoadAreaObjects(LibraryLower, Environment);

                // Areas To Unload
                UnloadAreaObjects(Garden, LowPriority);
                UnloadAll(LibraryLower);
                UnloadAll(LibraryUpper);
                UnloadAreaObjects(LibraryUpper, Environment);
                break;
            case Areas.Graveyard:
                // Areas To Load
                LoadAll(LibraryLower);
                LoadAll(LibraryUpper);
                LoadAll(CourtYard);
                LoadAll(Tomb);
                LoadAreaObjects(MainEntrance, HighPriority);
                LoadAreaObjects(MainEntrance, Environment);

                // Areas To Unload
                UnloadAreaObjects(MainEntrance, LowPriority);
                UnloadAreaObjects(MainEntrance, Environment);
                UnloadAll(Garden);
                UnloadAreaObjects(Garden, Environment);
                UnloadAreaObjects(CathedralHallLower, LowPriority);
                break;
            case Areas.Tomb:
                // Areas To Load
                LoadAll(Graveyard);
                LoadAll(MainEntrance);
                LoadAreaObjects(Garden, HighPriority);
                LoadAreaObjects(Garden, Environment);
                LoadAreaObjects(CathedralHallLower, HighPriority);
                LoadAreaObjects(CathedralHallLower, Environment);
                LoadAreaObjects(LibraryLower, HighPriority);
                LoadAreaObjects(LibraryLower, Environment);
                LoadAreaObjects(LibraryUpper, Environment);

                // Areas To Unload
                UnloadAreaObjects(CathedralHallLower, LowPriority);
                UnloadAreaObjects(Garden, LowPriority);
                UnloadAreaObjects(LibraryLower, LowPriority);
                UnloadAll(LibraryUpper);
                break;
            case Areas.LibraryLower:
                // Areas To Load
                LoadAreaObjects(CourtYard, HighPriority);
                LoadAll(Graveyard);
                LoadAreaObjects(Graveyard, Environment);

                // Areas To Unload
                UnloadAll(MainEntrance);
                UnloadAreaObjects(MainEntrance, Environment);
                UnloadAll(Garden);
                UnloadAreaObjects(Garden, Environment);
                UnloadAreaObjects(CathedralHallLower, LowPriority);
                UnloadAll(Tomb);
                UnloadAll(CourtYard);
                break;
        }
    }

    void LoadAllAreas()
    {
        LoadAll(CourtYard);
        LoadAll(Graveyard);
        LoadAll(MainEntrance);
        LoadAll(Garden);
        LoadAll(LibraryLower);
        LoadAll(CathedralHallLower);
        LoadAll(LibraryUpper);
        LoadAll(Tomb);
    }

    #region DICTIONARY CREATION
    private void InitaliseDictionary()
    {
        AddArea(Areas.Courtyard);
        AddArea(Areas.Graveyard);
        AddArea(Areas.MainEntrance);
        AddArea(Areas.Garden);
        AddArea(Areas.Kitchen);
        AddArea(Areas.LibraryLower);
        AddArea(Areas.BishopsQuarters);
        AddArea(Areas.CathedralHallUpper);
        AddArea(Areas.CathedralHallLower);
        AddArea(Areas.Tomb);
        AddArea(Areas.LibaryUpper);
    }

    private void AddArea(Areas area)
    {
        AreaObjects.Add(area, new Dictionary<AreaPriority, AreaPriorityObjects>());
        AreaObjects[area].Add(AreaPriority.LOW_PRIORITY, new AreaPriorityObjects());
        AreaObjects[area].Add(AreaPriority.HIGH_PRIORITY, new AreaPriorityObjects());
        AreaObjects[area].Add(AreaPriority.ENVIRONMENT, new AreaPriorityObjects());
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
        LoadAreaObjects(area, Environment);
    }

    private void UnloadAll(Areas area)
    {
        UnloadAreaObjects(area, LowPriority);
        UnloadAreaObjects(area, HighPriority);
        //UnloadAreaObjects(area, Environment);
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
