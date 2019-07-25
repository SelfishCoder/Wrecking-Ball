using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace WreckingBall
{
    /// <summary>
    /// This class controls the movement and behaviour of the crosshair.
    /// </summary>
    public class Crosshair : MonoBehaviour
    {
        [SerializeField] private CrosshairState crosshairState;
        [SerializeField] private Vector3 deflection;
        [SerializeField] private float speed;
        private Vector2 canvasReferenceResolution = new Vector2(1080,1920);
        private Vector3 newPosition;
        private float timeElapsed;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            speed = 2f;
            CalculateDeflectionOfCrosshair();
            timeElapsed = 0;
            newPosition = Vector3.zero;
            crosshairState = CrosshairState.OnMoveHorizontally;
        }

        /// <summary>
        /// This method calculates how much the crosshair should deflect on each axis.
        /// </summary>
        private void CalculateDeflectionOfCrosshair()
        {
            float deflectionX = canvasReferenceResolution.x - (this.GetComponent<RectTransform>().sizeDelta.x / 2);
            float deflectionY = canvasReferenceResolution.y - (this.GetComponent<RectTransform>().sizeDelta.y / 2);
            deflection = new Vector3(deflectionX, deflectionY, 0);
        }

        /// <summary>
        /// This methods moves the crosshair at the x axis with the specified deflection.
        /// </summary>
        private void MoveCrosshairHorizontally(float horizontalDeflection)
        {
            newPosition.x = Mathf.Sin(timeElapsed * speed) * horizontalDeflection;
            transform.localPosition = newPosition;
        }

        /// <summary>
        /// This methods moves the crosshair at the y axis with the specified deflection.
        /// </summary>
        private void MoveCrosshairVertically(float verticalDeflection)
        {
            newPosition.y = Mathf.Sin(timeElapsed * speed) * verticalDeflection;
            transform.localPosition = newPosition;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            switch (crosshairState)
            {
                case CrosshairState.OnMoveHorizontally:
                    timeElapsed += Time.deltaTime;
                    MoveCrosshairHorizontally(deflection.x);
                    if (InputManager.IsScreenTapted())
                    {
                        crosshairState = CrosshairState.OnMoveVertically;
                        timeElapsed = 0;
                    }

                    break;

                case CrosshairState.OnMoveVertically:
                    timeElapsed += Time.deltaTime;
                    MoveCrosshairVertically(deflection.y);
                    if (InputManager.IsScreenTapted())
                    {
                        crosshairState = CrosshairState.OnPowerSelection;
                        timeElapsed = 0;
                    }

                    break;

                case CrosshairState.None:
                    break;
                case CrosshairState.OnPowerSelection:
                    break;
                default:
                    break;
            }
        }
    }
}