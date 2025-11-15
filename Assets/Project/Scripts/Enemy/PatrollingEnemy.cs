using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingEnemy : AbstractEnemy
{
    private int currentIndex = 0;

    [SerializeField]
    List<Transform> patrolPoints;
    bool chasingPlayer;
    public Transform PlayerTransform = null;
    public Vector2 playerLastKnownPosition;
    
    [SerializeField] float chasingStartDistance = .2f;
    [SerializeField] float chasingStopDistance = 0.2f;

    void Start()
    {
        Initialize();
    }

    public override void EnemyBehavior()
    {
        setChasingPlayerStatus();
        

        if (!chasingPlayer)
        {
            Patrol();
        }
        else
        {
            ChasePlayer();
        }
    }

    private void setChasingPlayerStatus()
    {
        if (!chasingPlayer)
        {
            // placeholder logic for now - if close to player, chase
            if (PlayerTransform == null)
            {
                PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            }
            float distanceToPlayer = Vector2.Distance(transform.position, PlayerTransform.position);
            if (distanceToPlayer < chasingStartDistance)
            {
                chasingPlayer = true;
                playerLastKnownPosition = PlayerTransform.position;
            }
            
        }
        if (chasingPlayer)
        {
            // stop chasing when reached last known position
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < chasingStopDistance)
            {
                chasingPlayer = false;
            }
        }

    }

    private void Patrol()
    {
        // if close to player, chase  player.
        // if very close to player, attack player periodically.
        // if was chasing player, but lost player (details?), return to patrol.
        // if loud unexpected sound was heard, investigate sound.
        
        // go to the next patrol point upon reaching the current one
        
        if (target == null || patrolPoints.Count == 0) return;

        navMeshAgent.SetDestination(target.position);

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.3f)
        {
            currentIndex = (currentIndex + 1) % patrolPoints.Count;
            target = patrolPoints[currentIndex];
        }
    }


    public void ChasePlayer()
    {
        navMeshAgent.SetDestination(playerLastKnownPosition);
    }

}
