using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawners : MonoBehaviour
{
    [Header("Player")]
    public Transform player;
    [SerializeField] NavMeshAgent playerAgent;

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

    public void ActiveSpawners(Transform player, List<Spawner> baseSpawns, List<Spawner> specialSpawns)
    {
        Vector2 pos = new Vector2(player.position.x, player.position.z);

        List<Spawner> tempListBase = new List<Spawner>();
        List<Spawner> tempListspecial = new List<Spawner>();

        foreach (Spawner bt in baseSpawns)
        {
            Vector2 spawnerPos = new Vector2(bt.position.x, bt.position.z);

            float dist = Vector2.Distance(pos, spawnerPos);

            if (dist < maxSpawningDistance)
            {
                tempListBase.Add(bt);
            }
        }

        foreach (Spawner st in specialSpawns)
        {
            Vector2 spawnerPos = new Vector2(st.position.x, st.position.z);

            float dist = Vector2.Distance(pos, spawnerPos);

            if (dist < maxSpawningDistance)
            {
                tempListspecial.Add(st);
            }
        }

        foreach (Spawner bt in tempListBase)
        {
            NavMeshPath path = new NavMeshPath();

            playerAgent.CalculatePath(bt.position, path);

            playerAgent.SetPath(path);

            float dist = playerAgent.remainingDistance;

            if (dist < maxPathingDistance)
            {
                if (!baseActiveSpawners.Contains(bt))
                {
                    baseActiveSpawners.Add(bt);
                }
            }
            else
            {
                if (baseActiveSpawners.Contains(bt))
                {
                    baseActiveSpawners.Remove(bt);
                }
            }
        }

        foreach (Spawner st in tempListspecial)
        {
            NavMeshPath path = new NavMeshPath();

            playerAgent.CalculatePath(st.position, path);

            playerAgent.SetPath(path);

            float dist = playerAgent.remainingDistance;

            if (dist < maxPathingDistance)
            {
                if (!specialActiveSpawners.Contains(st))
                {
                    specialActiveSpawners.Add(st);
                }
            }
            else
            {
                if (specialActiveSpawners.Contains(st))
                {
                    specialActiveSpawners.Remove(st);
                }
            }
        }
    }
}
