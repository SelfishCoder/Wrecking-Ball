using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

namespace WreckingBall
{
    /// <summary>
    /// This script controls the lifetime of the game.
    /// </summary>
    [DisallowMultipleComponent]
    public class GameManager : Singleton<GameManager>
    {
        private static GameState currentGameState;
        public static GameState CurrentGameState
        {
            get => currentGameState;
            set => currentGameState = value;
        }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            currentGameState = GameState.OnMainMenu;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            switch (currentGameState)
            {
                case GameState.OnMainMenu:
                    if (!SceneManager.IsCurrentScene("Main Menu"))
                        SceneManager.LoadScene("Main Menu");
                    break;

                case GameState.InGame:
                    if (PlayerStats.Lives == 0 )
                    {
                        if (LevelManager.Instance.LevelProgress.Equals(100))
                        {
                            currentGameState = GameState.Won;
                        }
                        else
                        {
                            currentGameState = GameState.Lost;
                        }
                    }
                    if (LevelManager.Instance.LevelProgress.Equals(100))
                    {
                        currentGameState = GameState.Won;
                    }
                    break;

                case GameState.OnPauseMenu:
                    break;

                case GameState.Won:
                    StartCoroutine(OnLevelWon());
                    break;
                
                case GameState.Lost:
                    UiManager.Instance.OpenLostPanel();
                    UiManager.Instance.CloseCrosshair();
                    currentGameState = GameState.None;
                    break;
                
                case GameState.None:
                    break;
            }
        }

        /// <summary>
        /// This method sets gamestate to none.
        /// Shows fireworks.
        /// Update Level score and shows.
        /// </summary>
        /// <returns>StartCoroutine</returns>
        private IEnumerator OnLevelWon()
        {
            currentGameState = GameState.None;
            yield return StartCoroutine(LevelManager.Instance.ShowFireworks());
            if (PlayerStats.Lives == 4)
                LevelManager.Instance.currentLevel.CollectedStarAmount = 3;
            else if (2 <= (PlayerStats.Lives) || PlayerStats.Lives <= 3)
                LevelManager.Instance.currentLevel.CollectedStarAmount = 2;
            else
                LevelManager.Instance.currentLevel.CollectedStarAmount = 1;
            UiManager.Instance.ResetWonPanel();
            UiManager.Instance.UpdateWonPanel();
            UiManager.Instance.OpenWonPanel();
            UiManager.Instance.CloseCrosshair();
        }
    }
}