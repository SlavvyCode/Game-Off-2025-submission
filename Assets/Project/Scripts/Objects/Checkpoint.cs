using System;
using General_and_Helpers;
using UnityEngine;

namespace LevelElements.Common
{
    public class Checkpoint : MonoBehaviour
    {
        public int index { get; private set; }  = -1;
        public string checkpointId { get; private set; }
        private ParticleSystem particleSystem;
        public static event Action OnCheckpointReached;
        private void Awake()
        {
            checkpointId = Util.GenerateID(gameObject);
        }

        private void Start()
        {
            //  can just drag it i was lazy
            particleSystem = GetComponentInChildren<ParticleSystem>(); 
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) 
            {
                LevelManager.Instance.SetCheckpoint(this); 
                // particle effect to tell the player it worked
                CheckpointParticles();
                OnCheckpointReached?.Invoke();
            }
        }

        private void CheckpointParticles()
        {
            particleSystem.Play();
        }
    
        public static Checkpoint FindByID(string id)
        {
            var checkpoints = GameObject.FindObjectsOfType<Checkpoint>(true); // include inactive
            foreach (var cp in checkpoints)
            {
                if (cp.checkpointId == id)
                    return cp;
            }

            return null; 
        }

        
        
        
    }
}
