using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Objects;
using Project.Scripts.Player;
using UnityEngine;

public class Rock : MonoBehaviour
{
    bool landed = false;
    Rigidbody2D rb;
    private Collider2D rockCollider;
    
    [SerializeField] private SoundSet throwSoundSet;


    
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rockCollider = GetComponent<Collider2D>();
    }
    void Update()
    {
        if (!landed && rb.linearVelocity.magnitude < 0.2f)
        {
            landed = true;
            OnLand();
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        //if other not player
        if (other.collider.CompareTag("Player"))
            return;
        
        
        
        // play collision sound
        AudioManager.Instance.PlaySound(throwSoundSet.GetRandom(), transform.position);

        
        
        
        
        if (landed)
            return;
        
        
        // bounce off walls/floors. actually no, that should be handled by physics.
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!landed)
            return;
        //check for player, add to inventory if room or if interactable
        //this way probably simplest.
        if (other.CompareTag("Player"))
        {
            PlayerThrow playerThrow = other.GetComponent<PlayerThrow>();
            playerThrow.AddRockToInventory();
            Destroy(gameObject);
        }
        

    }
    void OnLand()
    {
        
        // play landing sound
        AudioManager.Instance.PlaySound(throwSoundSet.GetRandom(), transform.position);
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        
        rb.bodyType = RigidbodyType2D.Kinematic;
        // optionally: become a pickup rock
        // or change sprite, or spawn dust, etc.

        Debug.Log("Rock landed!");
    }
    
    public void GetThrown(Collider2D playerCollider, Vector2 velocity)
    {
        rb.linearVelocity = velocity;
        rb.bodyType = RigidbodyType2D.Dynamic;
        landed = false;
        //even ignores triggers.
        Physics2D.IgnoreCollision(playerCollider, rockCollider, true);
        StartCoroutine(EnablePlayerCollisionLater(playerCollider));
    }
    
    
    private IEnumerator EnablePlayerCollisionLater(Collider2D playerCollider)
    {
        yield return new WaitForSeconds(0.2f);
        Physics2D.IgnoreCollision(playerCollider, rockCollider, false);
    }

}
