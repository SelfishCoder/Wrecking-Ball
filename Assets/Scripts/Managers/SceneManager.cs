using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WreckingBall
{
    /// <summary>
    /// This script controls Scenes and Levels of the game.
    /// </summary>
    public static class SceneManager
    {
        /// <summary>
        /// This method checks if the scene is equal to active scene by name of the scene.
        /// </summary>
        /// <param name="sceneName">Name of the scene.</param>
        /// <returns></returns>
        public static bool IsCurrentScene(string sceneName)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().Equals(UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName)))
                return true;
            return false;
        }

        /// <summary>
        /// This method loads the specified scene with the given scene name.
        /// </summary>
        /// <param name="sceneName"></param>
        public static void LoadScene(string sceneName)
        {
            int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).buildIndex;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// This method loads the specified scene with the given scene index.
        /// </summary>
        /// <param name="sceneIndex"></param>
        public static void LoadScene(int sceneIndex)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        }
        
        /// <summary>
        /// This method finds the currently active scene and returns the build index of it.
        /// </summary>
        /// <returns>Build index of the current scene.</returns>
        public static int GetCurrentSceneIndex()
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        }

        /// <summary>
        /// This method finds the currently active scene and returns the name of it.
        /// </summary>
        /// <returns>Name of the current scene.</returns>
        public static string GetCurrentSceneName()
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }
    }
}