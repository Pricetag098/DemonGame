using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Spawners : MonoBehaviour
{
    [Header("Spawn Location")]
    [HideInInspector] public List<Spawner> baseSpawners = new List<Spawner>();
    [HideInInspector] public List<Spawner> specialSpawners = new List<Spawner>();

    private DetectArea areaInfo;
    private Areas currentArea;

    private Area[] areas;
    private Dictionary<Areas, Area> AreaDictionary = new Dictionary<Areas, Area>();

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
    }

    public void UpdateActiveSpawners(Areas Id)
    {
        if(Id != areaInfo.CurrentArea)
        {
            Debug.Log("Updating Spawners");
            ResetSpawners();
            
            currentArea = Id;

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

            foreach (Area a in area.AdjacentAreas)
            {
                if(a.discovered == true)
                {
                    if (area.baseDepth > 0)
                    {
                        List<Spawner> clostSpawners = new List<Spawner>(a.baseSpawns);

                        foreach (Spawner s in clostSpawners)
                        {
                            s.distToArea = Vector2.Distance(s.position, a.position);
                        }

                        clostSpawners.Sort((p1, p2) => p1.distToArea.CompareTo(p2.distToArea));

                        int num = clostSpawners.Count;
                        if (num > a.baseDepth) { num = a.baseDepth; }

                        for (int i = 0; i < num; i++)
                        {
                            baseSpawners.Add(clostSpawners[i]);
                        }
                    }
                    if (area.specialDepth > 0)
                    {
                        List<Spawner> clostSpawners = new List<Spawner>(a.specialSpawns);

                        foreach (Spawner s in clostSpawners)
                        {
                            s.distToArea = Vector2.Distance(s.position, a.position);
                        }

                        clostSpawners.Sort((p1, p2) => p1.distToArea.CompareTo(p2.distToArea));

                        int num = clostSpawners.Count;
                        if (num > a.specialDepth) { num = a.specialDepth; }

                        for (int i = 0; i < num; i++)
                        {
                            specialSpawners.Add(clostSpawners[i]);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        foreach (var spawner in baseSpawners)
        {
            
            Gizmos.DrawCube(spawner.position, new Vector3(2, 2, 2));
        }

        Gizmos.color = Color.blue;
        foreach (var spawner in specialSpawners)
        {
            
            Gizmos.DrawCube(spawner.position, new Vector3(2, 2, 2));
        }
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