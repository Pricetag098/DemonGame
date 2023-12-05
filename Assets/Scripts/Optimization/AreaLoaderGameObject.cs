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
    Areas OutsideTomb = Areas.OutsideTomb;
    Areas LibraryUpper = Areas.LibaryUpper;
    Areas CathedralHallBack = Areas.CathedralHallBack;
    Areas InsideTomb = Areas.InsideTomb;

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
                LoadAll(OutsideTomb);
                LoadAreaObjects(InsideTomb, Environment);
                LoadAreaObjects(CathedralHallLower, HighPriority);
                LoadAreaObjects(CathedralHallLower, Environment);

                // Areas To Unload
                UnloadAreaObjects(Graveyard, LowPriority);
                UnloadAreaObjects(Graveyard, Environment);
                UnloadAreaObjects(CathedralHallLower, LowPriority);
                UnloadAll(LibraryLower);
                UnloadAreaObjects(LibraryLower, Environment);
                UnloadAll(LibraryUpper);
                UnloadAreaObjects(LibraryUpper, Environment);
                UnloadAll(InsideTomb);
                UnloadAll(CathedralHallBack);
                UnloadAll(CathedralHallUpper);
                UnloadAreaObjects(CathedralHallLower, LowPriority);
                break;
            case Areas.Garden:
                // Areas To Load
                LoadAll(MainEntrance);
                LoadAreaObjects(CourtYard, HighPriority);
                LoadAreaObjects(CourtYard, Environment);
                LoadAll(CathedralHallUpper);
                LoadAll(CathedralHallLower);

                // Areas To Unload
                UnloadAll(Graveyard);
                UnloadAreaObjects(Graveyard, Environment);
                UnloadAreaObjects(CourtYard, LowPriority);
                UnloadAll(LibraryLower);
                UnloadAreaObjects(LibraryLower, Environment);
                UnloadAll(LibraryUpper);
                UnloadAreaObjects(LibraryUpper, Environment);
                UnloadAll(OutsideTomb);
                UnloadAreaObjects(InsideTomb, Environment);
                UnloadAll(CathedralHallBack);
                break;
            case Areas.Courtyard:
                // Areas To Load
                LoadAll(Graveyard);
                LoadAll(MainEntrance);
                LoadAreaObjects(Garden, HighPriority);
                LoadAreaObjects(Garden, Environment);
                LoadAll(CathedralHallLower);
                LoadAll(CathedralHallUpper);
                LoadAll(CathedralHallBack);
                LoadAreaObjects(LibraryLower, Environment);
                LoadAreaObjects(InsideTomb, Environment);
                LoadAreaObjects(LibraryUpper, Environment);

                // Areas To Unload
                UnloadAreaObjects(Garden, LowPriority);
                UnloadAll(LibraryLower);
                UnloadAll(LibraryUpper);
                LoadAreaObjects(InsideTomb,HighPriority);
                break;
            case Areas.Graveyard:
                // Areas To Load
                LoadAll(LibraryLower);
                LoadAll(LibraryUpper);
                LoadAll(CourtYard);
                LoadAll(OutsideTomb);
                LoadAreaObjects(MainEntrance, HighPriority);
                LoadAreaObjects(MainEntrance, Environment);
                LoadAreaObjects(InsideTomb, Environment);
                LoadAreaObjects(CathedralHallLower, HighPriority);
                LoadAreaObjects(CathedralHallLower, Environment);

                // Areas To Unload
                UnloadAreaObjects(MainEntrance, LowPriority);
                UnloadAreaObjects(MainEntrance, Environment);
                UnloadAll(Garden);
                UnloadAreaObjects(Garden, Environment);
                UnloadAll(InsideTomb);
                UnloadAll(CathedralHallBack);
                UnloadAll(CathedralHallUpper);
                UnloadAreaObjects(CathedralHallLower, LowPriority);
                break;
            case Areas.OutsideTomb:
                // Areas To Load
                LoadAll(Graveyard);
                LoadAll(MainEntrance);
                LoadAll(InsideTomb);
                LoadAreaObjects(Garden, HighPriority);
                LoadAreaObjects(Garden, Environment);
                LoadAreaObjects(CathedralHallLower, HighPriority);
                LoadAreaObjects(CathedralHallLower, Environment);
                LoadAreaObjects(LibraryLower, HighPriority);
                LoadAreaObjects(LibraryLower, Environment);
                LoadAreaObjects(LibraryUpper, Environment);
                LoadAreaObjects(CathedralHallLower, HighPriority);

                // Areas To Unload
                UnloadAreaObjects(CathedralHallLower, LowPriority);
                UnloadAreaObjects(Garden, LowPriority);
                UnloadAreaObjects(LibraryLower, LowPriority);
                UnloadAll(LibraryUpper);
                UnloadAll(CathedralHallBack);
                UnloadAll(CathedralHallUpper);
                UnloadAreaObjects(CathedralHallLower, LowPriority);
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
                UnloadAll(OutsideTomb);
                UnloadAll(CourtYard);
                UnloadAreaObjects(InsideTomb, Environment);
                UnloadAll(CathedralHallBack);
                UnloadAll(CathedralHallUpper);
                UnloadAll(CathedralHallLower);
                UnloadAreaObjects(CathedralHallLower, Environment);
                break;
            case Areas.LibaryUpper:
                // Areas To Load
                LoadAll(Graveyard);
                LoadAreaObjects(Graveyard, Environment);
                LoadAll(CathedralHallUpper);
                LoadAreaObjects(CathedralHallLower, HighPriority);
                UnloadAreaObjects(CathedralHallLower, Environment);

                // Areas To Unload
                UnloadAll(MainEntrance);
                UnloadAreaObjects(MainEntrance, Environment);
                UnloadAll(Garden);
                UnloadAreaObjects(Garden, Environment);
                UnloadAll(OutsideTomb);
                UnloadAll(InsideTomb);
                UnloadAreaObjects(InsideTomb, Environment);
                UnloadAll(CourtYard);
                break;
            case Areas.InsideTomb:
                // Areas To Load
                LoadAreaObjects(Graveyard, HighPriority);
                LoadAreaObjects(Graveyard, Environment);
                LoadAreaObjects(MainEntrance, Environment);
                LoadAreaObjects(MainEntrance, HighPriority);
                LoadAll(CourtYard);
                LoadAreaObjects(CathedralHallLower, HighPriority);

                // Areas To Unload
                UnloadAll(Garden);
                UnloadAll(LibraryLower);
                UnloadAll(LibraryUpper);
                UnloadAreaObjects(Garden, Environment);
                UnloadAreaObjects(LibraryLower, Environment);
                UnloadAreaObjects(LibraryUpper, Environment);
                UnloadAreaObjects(MainEntrance, LowPriority);
                UnloadAreaObjects(Graveyard, LowPriority);
                UnloadAll(CathedralHallBack);
                UnloadAll(CathedralHallUpper);
                UnloadAreaObjects(CathedralHallLower, LowPriority);
                break;
            case Areas.CathedralHallLower:
                // Areas To Load
                LoadAll(CourtYard);
                LoadAll(Garden);
                LoadAll(CathedralHallUpper);
                LoadAll(CathedralHallBack);
                LoadAll(CathedralHallLower);
                LoadAll(MainEntrance);
                LoadAll(Graveyard);
                LoadAll(OutsideTomb);
                LoadAreaObjects(LibraryUpper, Environment);

                // Areas To Unload
                UnloadAll(LibraryLower);
                UnloadAreaObjects(LibraryLower, Environment);
                UnloadAll(LibraryUpper);
                UnloadAll(InsideTomb);
                break;
            case Areas.CathedralHallUpper:
                // Areas To Load
                LoadAreaObjects(CourtYard, HighPriority);
                LoadAll(LibraryUpper);
                LoadAll(LibraryLower);
                LoadAll(CathedralHallLower);
                LoadAll(CathedralHallBack);


                // Areas To Unload
                UnloadAll(Garden);
                UnloadAll(Graveyard);
                UnloadAreaObjects(Graveyard, Environment);
                UnloadAll(MainEntrance);
                UnloadAreaObjects(MainEntrance, Environment);
                UnloadAll(OutsideTomb);
                UnloadAreaObjects(OutsideTomb, Environment);
                UnloadAll(InsideTomb);
                UnloadAreaObjects(InsideTomb, Environment);
                UnloadAreaObjects(CourtYard, LowPriority);
                break;
            case Areas.CathedralHallBack:
                // Areas To Load
                LoadAll(CourtYard);
                LoadAll(Garden);

                // Areas To Unload
                UnloadAll(Graveyard);
                UnloadAreaObjects(Graveyard, Environment);
                UnloadAll(MainEntrance);
                UnloadAreaObjects(MainEntrance, Environment);
                UnloadAll(OutsideTomb);
                UnloadAreaObjects(OutsideTomb, Environment);
                UnloadAll(InsideTomb);
                UnloadAreaObjects(InsideTomb, Environment);
                UnloadAll(LibraryUpper);
                UnloadAll(LibraryLower);
                UnloadAreaObjects(LibraryLower, Environment);
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
        LoadAll(CathedralHallUpper);
        LoadAll(LibraryUpper);
        LoadAll(OutsideTomb);
        LoadAll(CathedralHallBack);
        LoadAll(InsideTomb);
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
        AddArea(Areas.OutsideTomb);
        AddArea(Areas.LibaryUpper);
        AddArea(Areas.CathedralHallBack);
        AddArea(Areas.InsideTomb);
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
