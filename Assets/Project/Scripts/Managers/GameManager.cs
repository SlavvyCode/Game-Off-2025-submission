using System;
using Project.Scripts.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace General_and_Helpers
{
    public enum pausedState { Playing, Paused }
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public pausedState PausedStateEnum { get; private set; } = pausedState.Playing;
        public PlayerInput PlayerInput;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
        }

        private void Start()
        {
            PlayerInput = FindObjectOfType<PlayerInput>();
        }

        public void SetPausedState(pausedState newState)
        {
            PausedStateEnum = newState;
        }
        //
        //
        // public void FinishLevel()
        // {
        //     pausedState newState = pausedState.Paused;
        //     SetPausedState(newState);
        //     UIManager.ShowLevelCompleteCanvas();
        //     Time.timeScale = 0;
        //     LevelManager.Instance.RemoveCurrLevelSaveData();
        // }
    

    
        public static void SaveGame()
        {
            // TODO
            // Save game data - currently just a placeholder, data isn't saved permanently to any file yet,
            // currently only works on an in-level basis (checkpoint, collectibles...)
        }
    
        
        public void SaveAndExitGame()
        {
            SaveGame();
            Application.Quit();
        }


        public void KillPlayer()
        {
            LevelManager.Instance.ResetAtCheckpoint();
        }
    }
}