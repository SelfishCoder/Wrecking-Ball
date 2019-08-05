using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace WreckingBall
{
    /// <summary>
    /// This class controls the levels of the game.
    /// </summary>
    [DisallowMultipleComponent]
    public class LevelManager : Singleton<LevelManager>
    {
        #region Variables

        [Header("Configuration")]
        public List<Level> levels;
        public GameObject[] GameObjectsShouldMoveBetweenScenes;

        [Header("Values")] 
        public Level currentLevel;
        public Level nextLevel;
        public int LevelProgress;

        [Header("References")] 
        [SerializeField] private GameObject Firework;

        #endregion
        
        /// <summary>
        /// Temporary Variables
        /// </summary>
        private int DestructedCubeCount;
        private float DestructedPercentage;
        
        /// <summary>
        /// This method loads specified level ascync and unloads the current level async.
        /// </summary>
        /// <param name="level">Level should be loaded.</param>
        /// <returns>null</returns>
        public IEnumerator LoadLevel(Level level)
        {
            UiManager.Instance.CloseCrosshair();
            Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(level.Name, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            SceneManager.MoveGameObjectsToScene(GameObjectsShouldMoveBetweenScenes, SceneManager.GetSceneByName(level.Name));
            LevelManager.Instance.currentLevel = level;
            LevelManager.Instance.nextLevel = LevelManager.Instance.levels[LevelManager.Instance.levels.IndexOf(level) + 1];
            LevelProgress = 0;
            PlayerStats.Lives = 5;
            level.Obstacle = GameObject.Find("Obstacle_"+level.Name);
            LevelManager.Instance.UpdateUi();
            GameManager.CurrentGameState = GameState.InGame;
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentScene);
        }
        
        /// <summary>
        /// This method loads the next level.
        /// </summary>
        public void LoadNextLevel()
        {
            UnlockNextLevel();
            StartCoroutine(LoadLevel(nextLevel));
        }

        /// <summary>
        /// This method unlocks the next level.
        /// </summary>
        private static void UnlockNextLevel()
        {
            LevelManager.Instance.nextLevel.IsUnlocked = true;
            UiManager.Instance.UpdateLevelButtons();
        }

        /// <summary>
        /// This method restarts current level by loading current scene.
        /// </summary>
        public void RestartLevel()
        {
            UiManager.Instance.CloseCrosshair();
            Scene CurrentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            foreach (GameObject go in GameObjectsShouldMoveBetweenScenes)
            {
                DontDestroyOnLoad(go);
            }
            LevelProgress = 0;
            PlayerStats.Lives = 5;
            LevelManager.Instance.UpdateUi();
            GameManager.CurrentGameState = GameState.InGame;
        }

        /// <summary>
        /// This method Calculates the level progress when the wrecking ball disabled.
        /// </summary>
        /// <returns>Level Progress</returns>
        public int CalculateLevelProgress()
        {
            DestructedCubeCount = 0;
            foreach (Transform chieldTransform in currentLevel.Obstacle.transform)
            {
                if (!chieldTransform.GetComponent<Rigidbody>().isKinematic)
                {
                    DestructedCubeCount++;
                }
            }
            DestructedPercentage = (float) DestructedCubeCount / currentLevel.Obstacle.transform.childCount;
            return Mathf.RoundToInt(DestructedPercentage*100);
        }

        public IEnumerator ShowFireworks()
        {
            // Isntantiate Firework at floor's position.
            GameObject FireWorkClone = Instantiate(Firework, new Vector3(0, -23.5f, 45f),Quaternion.Euler(-90,0,0));
            FireWorkClone.SetActive(true);
            yield return new WaitForSeconds(5f);
            FireWorkClone.SetActive(false);
        } 
        
        /// <summary>
        /// This method updates the UI when level is loaded.
        /// </summary>
        private void UpdateUi()
        {
            UiManager.Instance.CloseLevelsPanel();
            UiManager.Instance.CloseMainMenuPanel();
            UiManager.Instance.OpenIngamePanel();
            UiManager.Instance.OpenCrosshair();
            UiManager.Instance.CloseWonPanel();
            UiManager.Instance.CloseWonPanel();
            UiManager.Instance.UpdateCurrentAndNextLevel();
            UiManager.Instance.UpdateLifeCounter(PlayerStats.Lives);
        }
    }
}