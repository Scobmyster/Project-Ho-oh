using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System;

public class GathererUnit : MonoBehaviour
{

    private NavMeshAgent agent;
    public bool is_Idle;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 position, Action onArrivedAction)
    {
        agent.SetDestination(position);
        agent.stoppingDistance = 0.5f;
        if (HaveIReachedDestination())
        {
            onArrivedAction();
        }
    }

    void Update()
    {
        if(HaveIReachedDestination())
        {
            is_Idle = true;
        }
    }

    public bool HaveIReachedDestination()
    {
        if(!agent.pathPending)
        {
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                if(!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    
}
