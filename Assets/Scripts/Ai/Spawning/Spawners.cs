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

    private DetectArea areaInfo;

    private Area[] areas;
    public static Dictionary<Areas, Area> AreaDictionary = new Dictionary<Areas, Area>();

    private void Awake()
    {
        areaInfo = FindObjectOfType<DetectArea>();
        areas = FindObjectsOfType<Area>();

        foreach(Area a in areas)
        {
            if(a.AreaId == Areas.Courtyard) { AreaDictionary.Add(a.AreaId, a);  }
            else if(a.AreaId == Areas.Graveyard) { AreaDictionary.Add(a.AreaId, a);  }
            else if (a.AreaId == Areas.MainEntrance) { AreaDictionary.Add(a.AreaId, a);  }
            else if (a.AreaId == Areas.Garden) { AreaDictionary.Add(a.AreaId, a);  }
            else if (a.AreaId == Areas.Kitchen) { AreaDictionary.Add(a.AreaId, a);  }
            else if (a.AreaId == Areas.Library) { AreaDictionary.Add(a.AreaId, a);  }
            else if (a.AreaId == Areas.BishopsQuarters) { AreaDictionary.Add(a.AreaId, a);  }
            else if (a.AreaId == Areas.CathedralHallUpper) { AreaDictionary.Add(a.AreaId, a);  }
            else if (a.AreaId == Areas.CathedralHallLower) { AreaDictionary.Add(a.AreaId, a);  }
        }

        barriers = FindObjectsOfType<DestrcutibleObject>();
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

                        closestSpawners.Sort((p1, p2) => p1.distToArea.CompareTo(p2.distToArea));

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

                        closestSpawners.Sort((p1, p2) => p1.distToArea.CompareTo(p2.distToArea));

                        int num = closestSpawners.Count;
                        if (num > area.specialDepth) { num = area.specialDepth; }

                        for (int i = 0; i < num; i++)
                        {
                            specialSpawners.Add(closestSpawners[i]);
                        }
                    }
                }
            }

            baseSpawners = HelperFuntions.ShuffleList(baseSpawners);
            specialSpawners = HelperFuntions.ShuffleList(specialSpawners);
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
    Library,
    BishopsQuarters,
    CathedralHallUpper,
    CathedralHallLower,
}