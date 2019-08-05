using System;
using UnityEngine;
using UnityEngine.UI;
using WreckingBall;

namespace WreckingBall
{
    /// <summary>
    /// This script controls the UI System of the game.
    /// </summary>
    [DisallowMultipleComponent]
    public class UiManager : Singleton<UiManager>
    {
        #region Variables
        
        [Header("References to UI Elements")]
        [SerializeField] private GameObject levelsPanel;
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject crosshair;
        [SerializeField] private GameObject inGamePanel;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject wonPanel;
        [SerializeField] private GameObject lostPanel;
        [SerializeField] private Slider LevelProgressBar;
        [SerializeField] private Text lifeText;
        [SerializeField] private Text currentLevelText;
        [SerializeField] private Text nextLevelText;
       
        #endregion
        
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            levelsPanel = transform.FindReferenceOfChild("Levels_Panel");
            mainMenuPanel = transform.FindReferenceOfChild("MainMenu_Panel");
            inGamePanel = transform.FindReferenceOfChild("InGame_Panel");
            crosshair = transform.FindReferenceOfChild("Crosshair_Image");
            wonPanel = transform.FindReferenceOfChild("Won_Panel");
            lostPanel = transform.FindReferenceOfChild("Lost_Panel");
            pausePanel = transform.FindReferenceOfChild("Pause_Panel");
            OpenMainMenuPanel();
            CloseLevelsPanel();
            CloseCrosshair();
            CloseIngamePanel();
            CloseLostPanel();
            CloseWonPanel();
        }

        /// <summary>
        /// This method pauses the game.
        /// </summary>
        public void PauseGame()
        {
            if (GameManager.CurrentGameState.Equals(GameState.InGame))
            {
                Time.timeScale = 0;
                GameManager.CurrentGameState = GameState.OnPauseMenu;
            }
        }

        /// <summary>
        /// This method continues the game.
        /// </summary>
        public void ResumeGame()
        {
            Time.timeScale = 1;
            GameManager.CurrentGameState = GameState.InGame;
        }
        
        /// <summary>
        /// This method opens level selection panel.
        /// </summary>
        public void OpenLevelsPanel()
        {
            levelsPanel.SetActive(true);
            UpdateLevelButtons();
        }
        
        /// <summary>
        /// This method closes level selection panel.
        /// </summary>
        public void CloseLevelsPanel()
        {
            levelsPanel.SetActive(false);
        }

        /// <summary>
        /// This method opens main menu.
        /// </summary>
        public void OpenMainMenuPanel()
        {
            mainMenuPanel.SetActive(true);
        }

        /// <summary>
        /// This method closes main menu
        /// </summary>
        public void CloseMainMenuPanel()
        {
            mainMenuPanel.SetActive(false);
        }

        /// <summary>
        /// This method opens in game ui panel.
        /// </summary>
        public void OpenIngamePanel()
        {
            inGamePanel.SetActive(true);
        }

        /// <summary>
        /// This method closes in game ui panel.
        /// </summary>
        public void CloseIngamePanel()
        {
            inGamePanel.SetActive(false);
        }

        /// <summary>
        /// This method opens crosshair ui.
        /// </summary>
        public void OpenCrosshair()
        {
            crosshair.SetActive(true);
        }

        /// <summary>
        /// This method closes crosshair ui.
        /// </summary>
        public void CloseCrosshair()
        {
            crosshair.SetActive(false);
        }

        /// <summary>
        /// This method reloads the current scene with ui button.
        /// </summary>
        public void RestartLevel()
        {
            SceneManager.ReloadCurrentScene();
        }

        /// <summary>
        /// This method opens level won panel.
        /// </summary>
        public void OpenWonPanel()
        {
            wonPanel.SetActive(true);
        }

        /// <summary>
        /// This method closes level won panel.
        /// </summary>
        public void CloseWonPanel()
        {
            wonPanel.SetActive(false);
        }

        /// <summary>
        /// This method opens level lost panel.
        /// </summary>
        public void OpenLostPanel()
        {
            lostPanel.SetActive(true);
        }

        /// <summary>
        /// This method closes level lost panel.
        /// </summary>
        public void CloseLostPanel()
        {
            lostPanel.SetActive(false);
        }
        
        /// <summary>
        /// This method opens pause panel.
        /// </summary>
        public void OpenPausePanel()
        { 
            pausePanel.SetActive(true);
        }

        /// <summary>
        /// This method closes pause panel.
        /// </summary>
        public void ClosePausePanel()
        {
            pausePanel.SetActive(false);
        }
        
        /// <summary>
        /// This method updates the life text.
        /// </summary>
        /// <param name="lifeLeft">Amount of life left</param>
        public void UpdateLifeCounter(int lifeLeft)
        {
            lifeText.text = lifeLeft.ToString();
        }

        /// <summary>
        /// This method updates the level progress bar.
        /// </summary>
        /// <param name="Progress"></param>
        public void UpdateLevelProgress(int Progress)
        {
            LevelProgressBar.value = Progress;
        }
        
        /// <summary>
        /// This method updates the current and next level texts.
        /// </summary>
        public void UpdateCurrentAndNextLevel()
        {
            currentLevelText.text = LevelManager.Instance.currentLevel.Id.ToString();
            nextLevelText.text = LevelManager.Instance.nextLevel.Id.ToString();
        }

        /// <summary>
        /// This method updates the star count and lock state of the level buttons.
        /// </summary>
        public void UpdateLevelButtons()
        {
            foreach (Level level in LevelManager.Instance.levels)
            {
                for (int i = 1; i <= level.CollectedStarAmount; i++)
                {
                    level.levelButton.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
                }
                level.levelButton.interactable = level.IsUnlocked;
            }
        }

        /// <summary>
        /// This method updates the collected star amount of the level won panel.
        /// </summary>
        public void UpdateWonPanel()
        {
            for (int i = 0; i < LevelManager.Instance.currentLevel.CollectedStarAmount; i++)
            {
                wonPanel.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        
        /// <summary>
        /// This method resets stars of the level won panel.
        /// </summary>
        public void ResetWonPanel()
        {
            for (int i = 0; i < LevelManager.Instance.currentLevel.CollectedStarAmount; i++)
            {
                wonPanel.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}