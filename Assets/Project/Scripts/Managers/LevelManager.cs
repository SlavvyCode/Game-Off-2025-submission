using System.Collections;
using System.Collections.Generic;
using LevelElements.Common;
using Project.Scripts.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace General_and_Helpers
{
    public class LevelManager : MonoBehaviour
    {
        //accessed from uimanager
        public bool showLevelSelect= false;
        [SerializeField] GameObject _player;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private PlayerInput _playerInput;



        private Rigidbody2D playerRb;


        private Vector2 playerSpawnPosition = Vector2.zero;
        private Camera _mainCamera;

        // Singleton
        public static LevelManager Instance { get; private set; }


        private void Start()
        {
            _playerInput = _player.GetComponent<PlayerInput>();
            _playerInput.enabled = true;
            if(SceneManager.GetActiveScene().name != "MainMenu")
                _playerInput.SwitchCurrentActionMap("Player");
            else
                _playerInput.SwitchCurrentActionMap("UI");
        }

        private void Awake()
        {
            {
                if (Instance == null)
                {
                    Instance = this;
                }
                else
                {
                    Debug.Log("LevelManager already exists, destroying new instance.");
                    Destroy(gameObject);
                    return;
                }
            }


            playerRb = _player.GetComponent<Rigidbody2D>();

            SceneManager.activeSceneChanged += OnSceneChanged;
        
        }



  

        void OnSceneChanged(Scene current, Scene next)
        {
            if (LevelManager.Instance == null)
            {
                Debug.LogWarning("LevelManager instance is null.");
                return;
            }

            if(next.name == "MainMenu")
            {
                return;
            }
            
            
            // playerSpawnPosition = GameObject.Find("SpawnPoint").transform.position;

            //should all get done on its own
            // MoveToCheckpoint();
        }
    
    
        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void LoadMainMenuAtLevelSelect()
        {
            showLevelSelect = true;
            SceneManager.LoadScene("MainMenu");
        }

        public void LoadNextLevel()
        {
            //get this scene's name, trim "Level " from it, convert to int, add 1, load that scene
            string currentSceneName = SceneManager.GetActiveScene().name;
            int currentLevel = int.Parse(currentSceneName.Substring(5));
            int nextSceneIndex = currentLevel + 1;
            SceneManager.LoadScene("Level " + nextSceneIndex);
        }


        //should all get done on its own
        // private void MoveToCheckpoint()
        // {
            // RefreshRefs();
            
            //find in playerprefs
            
            // var checkpointPosX = PlayerPrefs.GetFloat(
            
            // _player.transform.position = ;
            
            
            // playerRb.linearVelocity = Vector2.zero;
        // }





        //called on player death
        public IEnumerator RespawnPlayer()
        {
            yield return StartCoroutine(_uiManager.FadeToBlack());

            string currentScene = SceneManager.GetActiveScene().name;
            StopAllCoroutines();
            SceneManager.LoadScene(currentScene);
        
        }

        
    
    
    
        public void SetActionMapToUI()
        {
            _playerInput.SwitchCurrentActionMap("UI");
        }
    
        public void SetActionMapToPlayer()
        {
            _playerInput.SwitchCurrentActionMap("Player");
        }


        private void RefreshRefs()
        {
            _player = GameObject.FindWithTag("Player");
            playerRb = _player.GetComponent<Rigidbody2D>();
            _playerInput = _player.GetComponent<PlayerInput>();
            _mainCamera = Util.FindMainCamera();
        }
        
        
        public void RestartLevelAndDeleteSaveData()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            PlayerPrefs.DeleteAll();
            StopAllCoroutines();
            SceneManager.LoadScene(currentScene);
        }


        public void ResetAtCheckpoint()
        {
            StartCoroutine(RespawnPlayer());
        }
    }

}