using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingEnemy : AbstractEnemy
{
    

    [SerializeField]
    List<Transform> patrolPoints;
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyBehavior();
        
    }

    public override void EnemyBehavior()
    {
        // move between patrol points
        // Debug.Log("navMeshAgent.isOnNavMesh + " + navMeshAgent.isOnNavMesh);
        // if (!navMeshAgent.pathPending && navMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
        // {
            // Debug.LogWarning("Path not reachable!");
        // }

        
        // go to the next patrol point upon reaching the current one
        
        navMeshAgent.SetDestination(target.position);

    }
}
