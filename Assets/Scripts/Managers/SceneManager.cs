using UnityEngine.SceneManagement;

namespace WreckingBall
{
    /// <summary>
    /// This script controls Scenes and Levels of the game.
    /// </summary>
    public static class SceneManager
    {
        /// <summary>
        /// Checks if the scene is equal to active scene by name of the scene.
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
        /// This method loads the specified scene with the given scene index.
        /// </summary>
        /// <param name="sceneName"></param>
        public static void LoadScene(string sceneName)
        {
            int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).buildIndex;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        }

        public static void LoadLevel()
        {
        }
    }
}