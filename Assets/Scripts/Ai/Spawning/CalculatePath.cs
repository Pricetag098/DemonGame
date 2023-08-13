using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using Unity.Burst;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;
using Unity.Mathematics;
using Unity.Collections;

public class CalculatePathNavMeshJobs : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] bool useJobs;

    private NavMeshQuery query;
    private float3 extents;
    private Dictionary<int, float3[]> allPaths;
    private List<NativeArray<int>> statusOutput;
    private List<NativeArray<float3>> results;
    private List<NavMeshQuery> queries;
    private NavMeshWorld navmeshWorld;
    private List<JobHandle> jobHandles;


    private void Awake()
    {
        extents = new float3 (10, 10, 10);
        allPaths = new Dictionary<int, float3[]>();
        statusOutput = new List<NativeArray<int>>();
        results = new List<NativeArray<float3>>();
        queries = new List<NavMeshQuery>();
        jobHandles = new List<JobHandle>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if(useJobs == true)
        {
            Execute();
        }
        else
        {
            Execute();
        }
    }

    public void Execute()
    {

    }
}

[BurstCompile]
public struct CalculatePathsJob : IJob
{
    PathQueryStatus status;
    PathQueryStatus returningStatus;
    public NavMeshQuery query;
    public NavMeshLocation nm_fromLocation;
    public NavMeshLocation nm_ToLocation;
    public float3 fromLocation;
    public float3 ToLocation;
    public float3 extents;
    public int maxIteration;
    public NativeArray<float3> result;
    public NativeArray<int> statusOutput;
    public int maxPathSize;

    public void Execute()
    {
        nm_fromLocation = query.MapLocation(fromLocation, extents, 1);
        nm_ToLocation = query.MapLocation(ToLocation, extents, 1);

        if(query.IsValid(nm_fromLocation) && query.IsValid(nm_ToLocation))
        {
            status = query.BeginFindPath(nm_fromLocation, nm_ToLocation, -1);
            if(status == PathQueryStatus.InProgress)
            {
                query.UpdateFindPath(maxIteration, out int interationPerformed);
            }
            if(status == PathQueryStatus.Success)
            {
                status = query.EndFindPath(out int polygonSize);
                NativeArray<NavMeshLocation> res = new NativeArray<NavMeshLocation>(polygonSize, Allocator.Temp);
                NativeArray<StraightPathFlags> straightPathFlag = new NativeArray<StraightPathFlags>(maxPathSize, Allocator.Temp);
                NativeArray<float> vertexSide = new NativeArray<float>(maxPathSize, Allocator.Temp);
                NativeArray<PolygonId> polys = new NativeArray<PolygonId>(polygonSize, Allocator.Temp);
                int straightPathCount = 0;
                query.GetPathResult(polys);
                returningStatus = PathUtils.FindStraightPath(
                    query,
                    fromLocation,
                    ToLocation,
                    polys,
                    polygonSize,
                    ref res,
                    ref straightPathFlag,
                    ref vertexSide,
                    ref straightPathCount,
                    maxPathSize
                    );

                if(returningStatus == PathQueryStatus.Success)
                {
                    int fromKey = ((int)fromLocation.x + (int)fromLocation.y + (int)fromLocation.z) * maxPathSize;
                    int toKey = ((int)ToLocation.x + (int)ToLocation.y + (int)ToLocation.z) * maxPathSize;
                    int key = fromKey + toKey;
                    statusOutput[0] = 1;
                    statusOutput[1] = key;
                    statusOutput[3] = straightPathCount;

                    for (int i = 0; i < straightPathCount; i++)
                    {
                        result[i] = (float3)res[i].position;
                    }
                }

                res.Dispose();
                straightPathFlag.Dispose();
                polys.Dispose();
                vertexSide.Dispose();
            }
        }
    }
}