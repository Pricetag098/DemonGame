using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiTester : MonoBehaviour
{
    AiAgent[] agents;
    TestAgentThing[] things;
    [SerializeField] Transform target;
    // Start is called before the first frame update
    void Start()
    {
        agents = FindObjectsOfType<AiAgent>();
        things = FindObjectsOfType<TestAgentThing>();
        Run();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [ContextMenu("Run")]
    public void Run()
    {
        foreach (AiAgent agent in agents)
        {
            agent.UpdatePath(target);
        }
        foreach (TestAgentThing thing in things)
        {
            thing.GetComponent<NavMeshAgent>().SetDestination(target.position);
        }
    }
}
