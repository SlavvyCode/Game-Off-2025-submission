using System;
using General_and_Helpers;
using Project.Scripts.Objects;
using UnityEngine;

namespace LevelElements.Common
{
    public class Checkpoint : MonoBehaviour
    {
        private ParticleSystem particleSystem;
        public static event Action OnCheckpointReached;
        Vector2 respawnPos;
        private bool used = false;
        SaveableObject saveID;
        private void Awake()
        {
            saveID = GetComponent<SaveableObject>();
        }

        private void Start()
        {
            particleSystem = GetComponentInChildren<ParticleSystem>(); 
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) 
            {
                CheckpointParticles();
                OnCheckpointReached?.Invoke();
                
                // Save that THIS checkpoint is the current one
                PlayerPrefs.SetString("currentCheckpointID", saveID.UniqueID);

                var respawnPos = transform.position;
                
                PlayerPrefs.SetFloat(saveID.UniqueID + "_x", respawnPos.x);
                PlayerPrefs.SetFloat(saveID.UniqueID + "_y", respawnPos.y);
                //can only be used once
                PlayerPrefs.SetInt(saveID.UniqueID, 1);
                // saves all changes to PlayerPrefs to ensure data persistence, not just for this checkpoint
                PlayerPrefs.Save();
            }
        }
        private void CheckpointParticles()
        {
            particleSystem.Play();
        }
    
        
    }
}
