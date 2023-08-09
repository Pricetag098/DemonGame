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

    private void Awake()
    {
        
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
public struct CalculatePathsJob : IJobParallelFor
{
    public void Execute(int index)
    {
        
    }
}