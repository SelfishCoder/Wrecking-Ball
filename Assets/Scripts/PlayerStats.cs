using System;
using UnityEngine;

namespace WreckingBall
{
    /// <summary>
    /// This class holds the values of the user.
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        private static int lives;
        public static int Lives
        {
            get
            {
                return lives;
            }
            set
            {
                if (0 >= value || value <= 5)
                {
                    UiManager.Instance.UpdateLifeCounter(value);
                    lives = value;
                }
            }
        }
    }
}
