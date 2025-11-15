using System.Collections;
using General_and_Helpers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.UI;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Project.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        
        
        [SerializeField] private GameObject pauseMenu;
        // [SerializeField] private GameObject levelCompleteCanvas;
        [SerializeField] private GameObject blackCanvas;
        [SerializeField] private UnityEngine.UI.Image blackPanel;
        [SerializeField] private float fadeInDuration = 0.2f;
        [SerializeField] private float fadeOutDuration = 0.2f;
        private InputAction _menuAction;
        [SerializeField] private GameObject playerHUD;
    
        // class used a lot for buttons in the main menu
        // [SerializeField] private GameObject mainMenu;
        // [SerializeField] private GameObject levelSelectCanvas;
        // [SerializeField] private GameObject TitleScreenCanvas;
        
        [SerializeField] private GameObject nextLevelButton;

        private TopDownController playerController;

    
   
        public static UIManager instance;
        public GameManager gameManager;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                // DontDestroyOnLoad(gameObject);
            }
            // else
            // {
                // Debug.Log("UIManager already exists, destroying this instance.");
                // DontDestroyOnLoad(gameObject); // Destroy duplicate UIManager
            // }
        }
        
        public void RegisterPlayer(TopDownController controller)
        {
            playerController = controller;

            var menuAction = playerController.PlayerInput.actions["Menu"];
            menuAction.performed += ctx => TogglePause();
        }

        private void TogglePause()
        {
            if (gameManager.PausedStateEnum == pausedState.Playing)
                PauseGame();
            else if (gameManager.PausedStateEnum == pausedState.Paused)
                ResumeGame();
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
            
            
        }


        
        private void Start()
        {
            // _playerObject = GameManager.Instance.Player;
            gameManager = GameManager.Instance;
            
            playerController = Object.FindFirstObjectByType<TopDownController>();
        
            InputActionMap playerActionMap = playerController.PlayerInput.actions.FindActionMap("Player", true);
            _menuAction = playerActionMap.FindAction("Menu");
            _menuAction.performed += ctx =>
            {
                if (gameManager.PausedStateEnum == pausedState.Playing)
                    PauseGame();
                else if (gameManager.PausedStateEnum == pausedState.Paused)
                    ResumeGame();
            };

            
            
            SceneManager.activeSceneChanged += OnSceneChanged;
            
            
            OnSceneChanged(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());


        }

        private void OnSceneChanged(Scene current, Scene next)
        {
            HideAllUI();
            if (next.name == "MainMenu")
            {
                LevelManager.Instance.SetActionMapToUI();
                //TODO
                // showTitleScreen();
                // if (LevelManager.Instance.showLevelSelect)
                // {
                //     ShowLevelSelect();
                //     LevelManager.Instance.showLevelSelect = false;
                // }

                return;
            }
            
            LevelManager.Instance.SetActionMapToPlayer();

            //else we're in one of the levels
            ShowGameHUD();
            ResumeGame();
            
            StartCoroutine(FadeFromBlack());
            // StartCoroutine(SpawnPlayer());
        }


    
    
        public void ShowPauseMenu()
        {
            pauseMenu.SetActive(true);
        }
    
        public void HidePauseMenu()
        {
            pauseMenu.SetActive(false);
        }
        //
        // public void ShowLevelCompleteCanvas()
        // {
        //     //if last level, don't show next level button
        //     if (SceneManager.GetActiveScene().name == "Level 3")
        //     {
        //         nextLevelButton.SetActive(false);
        //     }
        //     else
        //     {
        //         nextLevelButton.SetActive(true);
        //     }
        //     levelCompleteCanvas.SetActive(true);
        // }
    
        
        public void PauseGame()
        {
            Time.timeScale = 0;
            GameManager.Instance.SetPausedState(pausedState.Paused);
            ShowPauseMenu();
    
            //todo
            //  when paused, you can maybe make a cycling animation of stars in the background
        
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
            GameManager.Instance.SetPausedState(pausedState.Paused);
            HidePauseMenu();
        }

    
        // Scene & Level Management

        public IEnumerator FadeToBlack()
        {
            
            blackPanel.color = new Color(0f, 0f, 0f, 0f);
            blackCanvas.SetActive(true);
            blackPanel.gameObject.SetActive(true);
            float t = 0f;
            while (t < fadeOutDuration)
            {
                t += Time.unscaledDeltaTime;
                float alpha = EaseInOut(t / fadeOutDuration);
                blackPanel.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }

        public IEnumerator FadeFromBlack()
        {
            
            blackCanvas.SetActive(true);
            blackPanel.gameObject.SetActive(true);
            blackPanel.color = new Color(0f, 0f, 0f, 1);
            float t = 0f;
            while (t < fadeInDuration)
            {
                t += Time.unscaledDeltaTime;
                float alpha = EaseInOut(t / fadeInDuration);
                blackPanel.color = new Color(0, 0, 0, 1-alpha);
                yield return null;
            }
            blackCanvas.SetActive(false);
            blackPanel.gameObject.SetActive(false);
        }
        
        float EaseInOut(float t)
        {
            return t * t * (3f - 2f * t); // smoothstep
        }

        

        // public void ShowLevelSelect()
        // {
        //     HideAllUI();
        //     // Util.SetActiveWithParents(levelSelectCanvas, true);
        // }
        //

    

        public void HideAllUI()
        {
            // mainMenu.SetActive(false);
            // levelSelectCanvas.SetActive(false);
            // levelCompleteCanvas.SetActive(false);
            playerHUD.SetActive(false);
            pauseMenu.SetActive(false);
        }

    
        public void ShowGameHUD()
        {
            playerHUD.SetActive(true);
        }

        // public void showTitleScreen()
        // {
        //     Util.SetActiveWithParents(TitleScreenCanvas, true);
        // }


       
    }

  
}