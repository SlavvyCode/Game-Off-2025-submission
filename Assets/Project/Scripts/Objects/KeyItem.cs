using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Scripts.Objects
{
    public class KeyItem : MonoBehaviour
    {
        [FormerlySerializedAs("pairedButton")] [SerializeField] public InteractableLever pairedLever;
        private Collider2D keyCollider;
        
        private void Start()
        {
            
            keyCollider = GetComponent<Collider2D>();
            if (keyCollider == null)
            {
                Debug.LogError("KeyItem: No Collider2D found on " + gameObject.name);
            }
         
            
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            //if other tag not player
            if (!other.CompareTag("Player"))
            {
                return;
            }
            
            
            Inventory playerInventory = other.GetComponent<Inventory>();
            if (playerInventory != null)
            {
                playerInventory.AddKey(this);
                Destroy(gameObject);
            }
            
        }
    }
}