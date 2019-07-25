using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WreckingBall
{
    public static class LevelManager
    {
        //[SerializeField] private List<Level> Levels;
        private static int currentLevelId;
        private static int nextLevelId;
        public static void LoadNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetCurrentSceneIndex()+1);
        }

        public static IEnumerator FadeInToScene()
        {
            yield return null;
        }
    }
}