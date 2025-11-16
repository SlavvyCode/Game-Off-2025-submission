using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractEnemy : MonoBehaviour
{
    protected NavMeshAgent navMeshAgent;
    [SerializeField] protected SpriteRenderer sr;

    // where the enemy is going
    [SerializeField] protected Transform target;
    
    private void UpdateSpriteDirection()
    {
        Vector3 vel = navMeshAgent.velocity;

        // If standing still, do nothing (avoids flicker)
        if (Mathf.Abs(vel.x) < 0.1f)
            return;

        // Flip the sprite depending on movement direction
        sr.flipX = vel.x < 0;
    }

    

    protected void Initialize()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        sr = GetComponent<SpriteRenderer>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        
        if(navMeshAgent == null)
        {
            Debug.Log("NavMeshAgent not found on " + gameObject.name);
        }
    }
    
    void Update()
    {
        // navMeshAgent.SetDestination(target.position);
        EnemyBehavior();
        UpdateSpriteDirection(); 
        
    }

    public abstract void EnemyBehavior();


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // OnPlayerCollision(other.gameObject);
        }
    }
    
}
