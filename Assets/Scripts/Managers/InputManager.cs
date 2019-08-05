using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
