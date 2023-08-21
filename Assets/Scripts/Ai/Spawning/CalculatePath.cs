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
    private NavMeshQuery query;
    private NativeArray<int> statusOutput;
    private NativeArray<float3> result;
    private JobHandle jobHandle;

    public void InitialiseJob(NavMeshWorld world,float3 extents, int maxIterations, NavMeshAgent agent, float3 fromLocation, float3 toLocation)
    {
        statusOutput = new NativeArray<int>(1024, Allocator.Temp);
        result = new NativeArray<float3>(1024, Allocator.Temp);
        query = new NavMeshQuery(world, Allocator.Temp);
        jobHandle = new JobHandle();

        CalculatePathsJob job = new CalculatePathsJob()
        {
            query = this.query,
            fromLocation = fromLocation,
            ToLocation = toLocation,
            extents = extents,
            maxIteration = maxIterations,
            result = this.result,
            statusOutput = this.statusOutput,
            maxPathSize = 50
        };

        jobHandle = job.Schedule();
        jobHandle.Complete();

        // do stuff here i think

        statusOutput.Dispose();
        result.Dispose();
        query.Dispose();
    }
}

[BurstCompile]
public struct CalculatePathsJob : IJob
{
    /// <summary>
    /// Query status
    /// </summary>
    PathQueryStatus status;
    PathQueryStatus returningStatus; 

    /// <summary>
    /// Query
    /// </summary>
    public NavMeshQuery query;

    /// <summary>
    /// Finds the closest point and PolygonId on the NavMesh for a given world position.
    /// </summary>
    public NavMeshLocation nm_fromLocation;
    public NavMeshLocation nm_ToLocation;

    /// <summary>
    /// World position for which the closest point on the NavMesh needs to be found.
    /// </summary>
    public float3 fromLocation;
    public float3 ToLocation;

    /// <summary>
    /// Maximum distance, from the specified position, expanding along all three axes, within which NavMesh surfaces are searched.
    /// </summary>
    public float3 extents;

    /// <summary>
    /// Maximum number of nodes to be traversed by the search algorithm during this call.
    /// </summary>
    public int maxIteration;

    /// <summary>
    /// Array for storing vector3
    /// </summary>
    public NativeArray<float3> result;

    /// <summary>
    /// Data array to be filled with the sequence of NavMesh nodes that comprises the found path.
    /// </summary>
    public NativeArray<int> statusOutput;

    public int maxPathSize;

    public void Execute()
    {
        nm_fromLocation = query.MapLocation(fromLocation, extents, 1);
        nm_ToLocation = query.MapLocation(ToLocation, extents, 1);

        if (query.IsValid(nm_fromLocation) && query.IsValid(nm_ToLocation))
        {
            status = query.BeginFindPath(nm_fromLocation, nm_ToLocation, -1);
            if (status == PathQueryStatus.InProgress)
            {
                query.UpdateFindPath(maxIteration, out int interationPerformed);
            }
            if (status == PathQueryStatus.Success)
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

                if (returningStatus == PathQueryStatus.Success)
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