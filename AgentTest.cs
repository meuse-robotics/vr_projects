using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentTest : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] NavMeshAgent agent;

    void Update()
    {
        var dist = Vector3.Distance(this.transform.position, target.transform.position);
        if (dist < 80f)
        {
            agent.speed = dist / 10f + 2f;
        }
        else
        {
            agent.speed = 10f;
        }
        agent.SetDestination(target.position);
    }
}
