using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WreckingBall
{
    /// <summary>
    /// This script controls input of the game.
    /// </summary>
    public static class InputManager
    {
        /// <summary>
        /// This method controls whether the screen is tapted or not.
        /// </summary>
        /// <returns>True or false</returns>
        public static bool IsScreenTapted()
        {
            return Input.GetMouseButtonDown(0);
        }
    }
}