using UnityEngine;

namespace WreckingBall
{
    /// <summary>
    /// This script controls the lifetime of the game.
    /// </summary>
    [DisallowMultipleComponent]
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField, Tooltip("State of the game in the lifetime.")]
        private static GameState currentGameState = GameState.MainMenu;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            currentGameState = GameState.MainMenu;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    if (!SceneManager.IsCurrentScene("Main Menu"))
                        SceneManager.LoadScene("Main Menu");
                    break;

                case GameState.InGame:

                    break;

                case GameState.PauseMenu:
                    break;

                default:
                    break;
            }
        }
    }
}