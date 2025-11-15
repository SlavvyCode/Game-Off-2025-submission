using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractEnemy : MonoBehaviour
{
    protected NavMeshAgent navMeshAgent;

    // where the enemy is going
    [SerializeField] protected Transform target;
    
    
    

    protected void Initialize()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
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
