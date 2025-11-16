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

        [SerializeField] private List<LevelSaveData> _levelSaveDataList = new List<LevelSaveData>();
        [SerializeField] public LevelSaveData currentSceneSaveData { get; private set; }


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



        private void AssignLevelSaveData(Scene scene)
        {
            bool correctDataFound = false;
            // add if not present to lvl save data
            foreach (var levelSaveData in _levelSaveDataList)
            {
                if (levelSaveData.SceneName == scene.name)
                {
                    currentSceneSaveData = levelSaveData;
                    correctDataFound= true;
                    break;
                }
            }

            if (!correctDataFound)
            {
                currentSceneSaveData = new LevelSaveData();
                currentSceneSaveData.SceneName = scene.name;
                _levelSaveDataList.Add(currentSceneSaveData);
            }

        }

        void OnSceneChanged(Scene current, Scene next)
        {
            if (LevelManager.Instance == null)
            {
                Debug.LogWarning("LevelManager instance is null.");
                return;
            }

            AssignLevelSaveData(next);
            if(next.name == "MainMenu")
            {
                return;
            }
            playerSpawnPosition = GameObject.Find("SpawnPoint").transform.position;


            MoveToCheckpoint();
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

        public void SetCheckpoint(Checkpoint checkpoint)
        {
            if (currentSceneSaveData.LastCheckpointId == null ||
                checkpoint.index >= Checkpoint.FindByID(currentSceneSaveData.LastCheckpointId).index)
            {
                currentSceneSaveData.LastCheckpointId = checkpoint.checkpointId;
            }
        }


        private void MoveToCheckpoint()
        {
            RefreshRefs();
            //no save data or CP, ergo move to checkponit, ELSE 
            if (currentSceneSaveData == null || currentSceneSaveData.LastCheckpointId == null)
            {
                _player.transform.position = playerSpawnPosition;
                _mainCamera.transform.position = playerSpawnPosition;
            }
            else
            {
                // move to checkpoint
                Checkpoint checkpoint = Checkpoint.FindByID(currentSceneSaveData.LastCheckpointId);
                _player.transform.position = checkpoint.transform.position;
                _mainCamera.transform.position = checkpoint.transform.position;
                _player.transform.rotation = checkpoint.transform.rotation;
            }

            playerRb.linearVelocity = Vector2.zero;
        }





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
            //delete save data for this level
            //find the save data for this level and remove it from the list
            RemoveCurrLevelSaveData();
            
            //reload scene
            string currentScene = SceneManager.GetActiveScene().name;
            StopAllCoroutines();
            SceneManager.LoadScene(currentScene);
        }

        public void RemoveCurrLevelSaveData()
        {
            if(currentSceneSaveData != null)
                _levelSaveDataList.Remove(currentSceneSaveData);
        }


        public void ResetAtCheckpoint()
        {
            StartCoroutine(RespawnPlayer());
        }
    }

}