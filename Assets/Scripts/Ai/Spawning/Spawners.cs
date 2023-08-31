using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        baseSpawners = HelperFuntions.GetAllChildrenSpawnersFromParent(baseSpawner);
        specialSpawners = HelperFuntions.GetAllChildrenSpawnersFromParent(SpecialSpawner);
    }

    public Spawner GetBaseSpawner(int num)
    {
        return baseActiveSpawners[num];
    }
    public Spawner GetSpecialSpawner(int num)
    {
        return specialActiveSpawners[num];
    }
    public int CheckBaseSpawners(Transform player, NavMeshAgent playerAgent, DemonSpawner spawner, SpawnerManager sm)
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
    public int CheckSpecialSpawners(Transform player, NavMeshAgent playerAgent, DemonSpawner spawner, SpawnerManager sm)
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
}
