﻿namespace WreckingBall
{
    /// <summary>
    /// Constants for state of the game. States are controlled by Game Manager.
    /// </summary>
    public enum GameState
    {
        InGame,
        OnMainMenu,
        OnPauseMenu,
        Won,
        Lost,
        None
    }
}