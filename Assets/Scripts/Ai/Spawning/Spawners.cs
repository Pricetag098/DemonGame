using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Spawners : MonoBehaviour
{
    [Header("Distance Checks")]
    [SerializeField] float maxPathingDistance;
    [SerializeField] float maxSpawningDistance;

    [Header("Spawn Location")]
    [SerializeField] Transform baseSpawner;
    public List<Spawner> baseActiveSpawners = new List<Spawner>();
    public List<Spawner> baseSpawners = new List<Spawner>();

    [SerializeField] Transform SpecialSpawner;
    public List<Spawner> specialActiveSpawners = new List<Spawner>();
    public List<Spawner> specialSpawners = new List<Spawner>();

    private DetectArea areaInfo;
    private Areas currentArea;

    private Area[] areas;
    private Dictionary<Areas, Area> AreaDictionary = new Dictionary<Areas, Area>();

    private void Awake()
    {
        baseSpawners = HelperFuntions.GetAllChildrenSpawnersFromParent(baseSpawner);
        specialSpawners = HelperFuntions.GetAllChildrenSpawnersFromParent(SpecialSpawner);

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

    public Spawner GetBaseSpawner(int num)
    {
        return baseActiveSpawners[num];
    }
    public Spawner GetSpecialSpawner(int num)
    {
        return specialActiveSpawners[num];
    }
    public int CheckBaseSpawners(Transform player, NavMeshAgent playerAgent)
    {
        Vector2 pos = new Vector2(player.position.x, player.position.z);

        List<Spawner> list = new List<Spawner>();

        foreach (Spawner bt in baseSpawners)
        {
            Vector2 spawnerPos = new Vector2(bt.position.x, bt.position.z);

            float dist = Vector2.Distance(pos, spawnerPos);

            bt.Visited = false;

            if (dist < maxSpawningDistance)
            {
                list.Add(bt);
            }
            else if(baseActiveSpawners.Contains(bt))
            {
                bt.CanSpawn = false;
                baseActiveSpawners.Remove(bt);
            }
        }

        foreach (Spawner bt in list)
        {
            NavMeshPath path = new NavMeshPath();

            playerAgent.CalculatePath(bt.position, path);

            playerAgent.SetPath(path);

            float dist = playerAgent.remainingDistance;

            if (dist < maxPathingDistance)
            {
                if (!baseActiveSpawners.Contains(bt))
                {
                    bt.CanSpawn = true;
                    baseActiveSpawners.Add(bt);
                }
            }
            else
            {
                if (baseActiveSpawners.Contains(bt))
                {
                    bt.CanSpawn = false;
                    baseActiveSpawners.Remove(bt);
                }
            }
        }

        baseActiveSpawners = HelperFuntions.ShuffleList(baseActiveSpawners);

        return baseActiveSpawners.Count;
    }
    public int CheckSpecialSpawners(Transform player, NavMeshAgent playerAgent)
    {
        Vector2 pos = new Vector2(player.position.x, player.position.z);

        List<Spawner> list = new List<Spawner>();

        foreach (Spawner st in specialSpawners)
        {
            Vector2 spawnerPos = new Vector2(st.position.x, st.position.z);

            float dist = Vector2.Distance(pos, spawnerPos);

            st.Visited = false;

            if (dist < maxSpawningDistance)
            {
                list.Add(st);
            }
            else if (specialActiveSpawners.Contains(st))
            {
                st.CanSpawn = false;
                specialActiveSpawners.Remove(st);
            }
        }

        foreach (Spawner st in list)
        {
            NavMeshPath path = new NavMeshPath();

            playerAgent.CalculatePath(st.position, path);

            playerAgent.SetPath(path);

            float dist = playerAgent.remainingDistance;

            if (dist < maxPathingDistance)
            {
                if (!specialActiveSpawners.Contains(st))
                {
                    st.CanSpawn = true;
                    specialActiveSpawners.Add(st);
                }
            }
            else
            {
                if (specialActiveSpawners.Contains(st))
                {
                    st.CanSpawn = false;
                    specialActiveSpawners.Remove(st);
                }
            }
        }

        specialActiveSpawners = HelperFuntions.ShuffleList(specialActiveSpawners);

        return specialActiveSpawners.Count;
    }

    public Areas GetPlayerArea()
    {
        return areaInfo.CurrentArea;
    }

    public void UpdateActiveSpawners(Areas Id)
    {
        if(Id != areaInfo.CurrentArea)
        {
            Debug.Log("Updating Spawners");
            currentArea = Id;

            Area area = AreaDictionary[Id];

            baseActiveSpawners.Clear();
            specialActiveSpawners.Clear();

            foreach(Spawner s in area.baseSpawns)
            {
                baseActiveSpawners.Add(s);
            }

            foreach (Spawner s in area.specialSpawns)
            {
                specialActiveSpawners.Add(s);
            }

            foreach (Area a in area.AdjacentAreas)
            {
                if(area.baseDepth > 0)
                {
                    List<Spawner> clostSpawners = new List<Spawner>(a.baseSpawns);

                    foreach(Spawner s in clostSpawners)
                    {
                        s.distToArea = Vector2.Distance(s.position, a.position);
                    }

                    clostSpawners.Sort((p1, p2) => p1.distToArea.CompareTo(p2.distToArea));

                    for (int i = 0; i < a.baseDepth; i++)
                    {
                        baseActiveSpawners.Add(clostSpawners[i]);
                    }
                }
                if(area.specialDepth > 0)
                {
                    List<Spawner> clostSpawners = new List<Spawner>(a.specialSpawns);

                    foreach (Spawner s in clostSpawners)
                    {
                        s.distToArea = Vector2.Distance(s.position, a.position);
                    }

                    clostSpawners.Sort((p1, p2) => p1.distToArea.CompareTo(p2.distToArea));

                    for (int i = 0; i < a.baseDepth; i++)
                    {
                        specialActiveSpawners.Add(clostSpawners[i]);
                    }
                }
            }

            baseActiveSpawners = HelperFuntions.ShuffleList(baseActiveSpawners);
            specialActiveSpawners = HelperFuntions.ShuffleList(specialActiveSpawners);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        foreach (var spawner in baseActiveSpawners)
        {
            
            Gizmos.DrawCube(spawner.position, new Vector3(2, 2, 2));
        }

        Gizmos.color = Color.blue;
        foreach (var spawner in specialActiveSpawners)
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