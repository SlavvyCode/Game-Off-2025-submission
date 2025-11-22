using System;
using System.Collections.Generic;
using Project.Scripts.Sound;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PatrollingEnemy : AbstractEnemy
{
    private int currentIndex = 0;
    private float attackCooldown = 2f;

    [SerializeField]
    List<Transform> patrolPoints;
    bool chasingPlayer;
    public Transform PlayerTransform = null;
    public Vector2 playerLastKnownPosition;
    
    [SerializeField] float chasingStartDistance = .2f;
    [SerializeField] float chasingStopDistance = 0.2f;

    
    
    
    // when not chasing and patrolling, periodically hiss    
    
    // Periodically
    // when snake hears noise, angry warning hiss, (optional pause) go investigate

    [SerializeField]
    private SoundData hissSound;
    [SerializeField]
    private SoundData warningHissSound;
    
    
    [SerializeField] private float hissMinDelay = 3f;
    [SerializeField] private float hissMaxDelay = 8f;
    [SerializeField] private float warningDistance = 1.5f;

    private float nextHissTime = 0f;

    
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
        TryPlayPatrolHiss();

        
        // if close to player, chase  player.
        // if very close to player, attack player periodically.
        // if was chasing player, but lost player (details?), return to patrol.
        // if loud unexpected sound was heard, investigate sound.
        
        // go to the next patrol point upon reaching the current one
        
        if (target == null || patrolPoints.Count == 0) return;

        try
        {
            navMeshAgent.SetDestination(target.position);

            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.3f)
            {
                currentIndex = (currentIndex + 1) % patrolPoints.Count;
                target = patrolPoints[currentIndex];
            }

        }
        catch (Exception e)
        {
            Debug.LogError("NavMeshMissing");
        }
    }

    private void TryPlayPatrolHiss()
    {
        if (Time.time < nextHissTime) 
            return; // still on cooldown

        // otherwise regular ambience patrol hiss
        AudioManager.Instance.PlaySound(hissSound, transform.position);

        // random delay before next hiss to make it feel natural
        nextHissTime = Time.time + Random.Range(hissMinDelay, hissMaxDelay);
        
    }


    public void ChasePlayer()
    {
        navMeshAgent.SetDestination(playerLastKnownPosition);
    }


    public void AttackPlayer()
    {
        // no need, just run into them
    }
    
    
    

}
