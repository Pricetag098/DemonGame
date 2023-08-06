using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using Unity.Burst;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;

public class CalculatePath : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] bool useJobs;

    NavMeshWorld world;

    private void Awake()
    {
        world = NavMeshWorld.GetDefaultWorld();
    }

    private void Start()
    {
        NavMeshQuery query = new NavMeshQuery(world, Unity.Collections.Allocator.TempJob);

        //NavMeshLocation start = query.CreateLocation(Vector3.zero, PolygonId);

        //NavMeshLocation end = query.CreateLocation(Vector3.zero, PolygonId);

        //query.BeginFindPath(start, end, -1);
    }

    private void Update()
    {
        if(useJobs == true)
        {

        }
        else
        {

        }
    }

    public void Execute()
    {

    }
}

[BurstCompile]
public struct CalculatePathsJob : IJobParallelFor
{
    public void Execute(int index)
    {
        
    }
}