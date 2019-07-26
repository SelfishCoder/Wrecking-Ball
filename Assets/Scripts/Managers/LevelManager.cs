using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WreckingBall
{
    public class LevelManager : Singleton<LevelManager>
    {
        [Header("Configuration")]
        public List<Level> levels;
        
        [Header("Values")]
        [SerializeField] private Level currentLevel;
        [SerializeField] private Level nextLevel;

        public static void LoadNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetCurrentSceneIndex()+1);
        }

        public static IEnumerator FadeIntoScene()
        {
            yield return null;
        }

        public static int GetLevelCount()
        {
            return LevelManager.Instance.levels.Count;
        }

        public static void UnlockNextLevel()
        {
            LevelManager.Instance.nextLevel.IsUnlocked = true;
        }

        public void RestartLevel()
        {
            SceneManager.ReloadCurrentScene();
        }
    }
}