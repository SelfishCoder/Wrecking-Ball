using System;
using UnityEngine;

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

        private void Awake()
        {
            levelsPanel = FindReferenceOfGameobject("Levels_Panel");
            mainMenuPanel = FindReferenceOfGameobject("MainMenu_Panel");
            OpenMainMenuPanel();
            CloseLevelsPanel();
        }

        public void PauseGame()
        {
            OpenMainMenuPanel();
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            CloseMainMenuPanel();
            Time.timeScale = 1;
        }

        public void LoadLevel(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            GameManager.CurrentGameState = GameState.InGame;
        }

        public void LoadNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetCurrentSceneIndex() + 1);
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

        private GameObject FindReferenceOfGameobject(string referenceName)
        {
            foreach (Transform t in this.transform)
            {
                if (t.gameObject.name.Equals(referenceName))
                {
                    return t.gameObject;
                }
            }
            return null;
        }
    }
}