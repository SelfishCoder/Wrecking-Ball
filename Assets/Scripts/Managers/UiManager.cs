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
        [SerializeField] private GameObject levelsPanel;
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private Button levelButton;

        private void Awake()
        {
            levelsPanel = transform.FindReferenceOfChild("Levels_Panel");
            mainMenuPanel = transform.FindReferenceOfChild("MainMenu_Panel");
            OpenMainMenuPanel();
            CloseLevelsPanel();
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
        }
        
        public void OpenLevelsPanel()
        {
            levelsPanel.SetActive(true);
        }

        public void CloseLevelsPanel()
        {
            levelsPanel.SetActive(false);
        }

        public void OpenMainMenuPanel()
        {
            mainMenuPanel.SetActive(true);
        }

        public void CloseMainMenuPanel()
        {
            mainMenuPanel.SetActive(false);
        }
    }
}