using UnityEngine;

namespace WreckingBall
{
    /// <summary>
    /// This script controls the UI System of the game.
    /// </summary>
    [DisallowMultipleComponent]
    public class UiManager : Singleton<UiManager>
    {
        public void PauseGame()
        {
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
        }
    
    }
}
