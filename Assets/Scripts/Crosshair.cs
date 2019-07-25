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
        [SerializeField] private float crosshairHorizontalSpeed;
        [SerializeField] private float crosshairVerticalSpeed;
        [SerializeField] private int powerbarFillSpeed;
        private readonly Vector2 canvasReferenceResolution = new Vector2(1080, 1920);
        private Vector3 newPosition;
        private float timeElapsed;
        private float power;

        [Header("Refences to UI Elements")] [SerializeField]
        private Slider powerBarUI;

        [SerializeField] private Image powerImage;
        [SerializeField] private Text powerText;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            powerBarUI.gameObject.SetActive(false);
            powerImage.gameObject.SetActive(false);
            crosshairHorizontalSpeed = 2.5f;
            crosshairVerticalSpeed = 3f;
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
            float deflectionX = (((canvasReferenceResolution.x / 2) / 10) * 9) - (this.GetComponent<RectTransform>().sizeDelta.x / 2);
            float deflectionY = (((canvasReferenceResolution.y / 2) / 10) * 9) - (this.GetComponent<RectTransform>().sizeDelta.y / 2);
            deflection = new Vector3(deflectionX, deflectionY, 0);
        }

        /// <summary>
        /// This methods moves the crosshair at the x axis with the specified deflection.
        /// </summary>
        private void MoveCrosshairHorizontally(float horizontalDeflection)
        {
            newPosition.x = Mathf.Sin(timeElapsed * crosshairHorizontalSpeed) * horizontalDeflection;
            transform.localPosition = newPosition;
        }

        /// <summary>
        /// This methods moves the crosshair at the y axis with the specified deflection.
        /// </summary>
        private void MoveCrosshairVertically(float verticalDeflection)
        {
            newPosition.y = Mathf.Sin(timeElapsed * crosshairVerticalSpeed) * verticalDeflection;
            transform.localPosition = newPosition;
        }

        private void PowerPingPong()
        {
            timeElapsed++;
            power = Mathf.PingPong(timeElapsed * powerbarFillSpeed, 100f);
            powerBarUI.value = power;
            powerText.text = powerBarUI.value.ToString();
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
                        powerBarUI.gameObject.SetActive(true);
                        powerImage.gameObject.SetActive(true);
                    }

                    break;

                case CrosshairState.None:
                    break;
                case CrosshairState.OnPowerSelection:
                    PowerPingPong();
                    if (InputManager.IsScreenTapted())
                        crosshairState = CrosshairState.None;
                    break;
                default:
                    break;
            }
        }
    }
}