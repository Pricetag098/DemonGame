using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Spawners : MonoBehaviour
{
    [Header("Spawn Location")]
    public List<Spawner> baseSpawners = new List<Spawner>();
    public List<Spawner> specialSpawners = new List<Spawner>();

    [Header("Barriers")]
    public DestrcutibleObject[] barriers;

    private Area[] areas;
    public static Dictionary<Areas, Area> AreaDictionary;

    private void Awake()
    {
        AreaDictionary = new Dictionary<Areas, Area>();

        areas = FindObjectsOfType<Area>();

        foreach(Area a in areas)
        {
            switch(a.AreaId)
            {
                case Areas.Courtyard:
                    AreaDictionary.Add(a.AreaId, a);
                    break;
                case Areas.Graveyard:
                    AreaDictionary.Add(a.AreaId, a);
                    break;
                case Areas.MainEntrance:
                    AreaDictionary.Add(a.AreaId, a);
                    break;
                case Areas.Garden:
                    AreaDictionary.Add(a.AreaId, a);
                    break;
                case Areas.Kitchen:
                    AreaDictionary.Add(a.AreaId, a);
                    break;
                case Areas.LibraryLower:
                    AreaDictionary.Add(a.AreaId, a);
                    break;
                case Areas.LibaryUpper:
                    AreaDictionary.Add(a.AreaId, a);
                    break;
                case Areas.BishopsQuarters:
                    AreaDictionary.Add(a.AreaId, a);
                    break;
                case Areas.CathedralHallUpper:
                    AreaDictionary.Add(a.AreaId, a);
                    break;
                case Areas.CathedralHallLower:
                    AreaDictionary.Add(a.AreaId, a);
                    break;
                case Areas.OutsideTomb:
                    AreaDictionary.Add(a.AreaId, a);
                    break;
                case Areas.InsideTomb:
                    AreaDictionary.Add(a.AreaId, a);
                    break;
                case Areas.CathedralHallBack:
                    AreaDictionary.Add(a.AreaId, a);
                    break;
            }
        }
    }

    public void UpdateActiveSpawners(Areas Id , Areas CurrentArea)
    {
        if(Id != CurrentArea)
        {
            ResetSpawners();

            Area area = AreaDictionary[Id];

            baseSpawners.Clear();
            specialSpawners.Clear();

            foreach(Spawner s in area.baseSpawns)
            {
                baseSpawners.Add(s);
            }

            foreach (Spawner s in area.specialSpawns)
            {
                specialSpawners.Add(s);
            }

            foreach (Optional<AreaConnect> a in area.OptionalAreas)
            {
                if(a.Value.Area.discovered == true && a.Value.Open == true)
                {
                    if (area.baseDepth > 0)
                    {
                        List<Spawner> closestSpawners = new List<Spawner>(a.Value.Area.baseSpawns);

                        foreach (Spawner s in closestSpawners)
                        {
                            s.distToArea = Vector2.Distance(s.position, a.Value.Area.position);
                        }

                        closestSpawners.Sort((p1, p2) => p2.distToArea.CompareTo(p1.distToArea));

                        int num = closestSpawners.Count;
                        if (num > area.baseDepth) { num = area.baseDepth; }

                        for (int i = 0; i < num; i++)
                        {
                            baseSpawners.Add(closestSpawners[i]);
                        }
                    }
                    if (area.specialDepth > 0)
                    {
                        List<Spawner> closestSpawners = new List<Spawner>(a.Value.Area.specialSpawns);

                        foreach (Spawner s in closestSpawners)
                        {
                            s.distToArea = Vector2.Distance(s.position, a.Value.Area.position);
                        }

                        closestSpawners.Sort((p1, p2) => p2.distToArea.CompareTo(p1.distToArea));

                        int num = closestSpawners.Count;
                        if (num > area.specialDepth) { num = area.specialDepth; }

                        for (int i = 0; i < num; i++)
                        {
                            specialSpawners.Add(closestSpawners[i]);
                        }
                    }
                }
            }
        }
    }

    public void ResetSpawners()
    {
        foreach(Spawner s in baseSpawners)
        {
            s.Visited = false;
        }
        foreach (Spawner s in specialSpawners)
        {
            s.Visited = false;
        }

        baseSpawners = HelperFuntions.ShuffleList(baseSpawners);
        specialSpawners = HelperFuntions.ShuffleList(specialSpawners);
    }


    public static bool GetDictionaryArea(Areas Id, out Area a)
    {
        a = AreaDictionary[Id];
        if(a is null) return false;

        return true;
    }
}

public enum Areas
{
    Null,
    Courtyard,
    Graveyard,
    MainEntrance,
    Garden,
    Kitchen,
    LibraryLower,
    BishopsQuarters,
    CathedralHallUpper,
    CathedralHallLower,
    OutsideTomb,
    LibaryUpper,
    CathedralHallBack,
    InsideTomb
}