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
    [DisallowMultipleComponent,RequireComponent(typeof(Animator))]
    public class Crosshair : MonoBehaviour
    {
        [Header("Crosshair Properties")]
        [SerializeField] private CrosshairState crosshairState;
        [SerializeField] private Vector3 deflection;
        [SerializeField] private float crosshairHorizontalSpeed = 3.25f;
        [SerializeField] private float crosshairVerticalSpeed = 3.5f;
        [SerializeField] private float powerbarFillSpeed = 0.03f;
        
        [Header("References to UI Elements")] 
        [SerializeField] private Slider powerBarSlider;
        [SerializeField] private Image horizontalAxisImage;
        [SerializeField] private Image verticalAxisImage;
        [SerializeField] private Image powerImage;
        [SerializeField] private Text powerText;
        [SerializeField] private Animator crosshairAnimator;
        
        private readonly Vector2 canvasReferenceResolution = new Vector2(1080, 1920);
        private Vector3 newPosition = Vector3.zero;
        private float timeElapsed;
        private static float power;
        public static float Power {
            get
            {
                if (power >= 99.5f)
                {
                    return 100f;
                }
                else
                {
                    return power;
                }
            }
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Awake()
        {
            crosshairAnimator = this.GetComponent<Animator>();
            CalculateAndSetDeflectionOfCrosshair();
        }

        /// <summary>
        /// This method calculates how much the crosshair should deflect on each axis.
        /// </summary>
        private void CalculateAndSetDeflectionOfCrosshair()
        {
            float deflectionX = (((canvasReferenceResolution.x / 2) / 10) * 9.75f) - (this.GetComponent<RectTransform>().sizeDelta.x / 2);
            float deflectionY = (((canvasReferenceResolution.y / 2) / 10) * 8.0f) - (this.GetComponent<RectTransform>().sizeDelta.y / 2);
            deflection = new Vector3(deflectionX, deflectionY, 0);
        }

        /// <summary>
        /// This methods moves the crosshair at the x axis with the specified deflection.
        /// </summary>
        private void MoveCrosshairHorizontally(float horizontalDeflection)
        {
            newPosition.x = Mathf.Sin(timeElapsed * crosshairHorizontalSpeed) * horizontalDeflection;
            transform.localPosition = newPosition;
            timeElapsed += Time.deltaTime;
        }

        /// <summary>
        /// This methods moves the crosshair at the y axis with the specified deflection.
        /// </summary>
        private void MoveCrosshairVertically(float verticalDeflection)
        {
            newPosition.y = Mathf.Sin(timeElapsed * crosshairVerticalSpeed) * verticalDeflection;
            transform.localPosition = newPosition;
            timeElapsed += Time.deltaTime;
        }
        
        private void PowerPingPong()
        {
            //This statement changes the value of power between 0 and 100 gradually.
            power = Mathf.RoundToInt(Mathf.Abs(Mathf.Sin(timeElapsed * powerbarFillSpeed) * 100));
            /*
                //This statement changes the value of power between 0 and 100 gradually. (Alternative)
                power = Mathf.PingPong(timeElapsed * powerbarFillSpeed, 100f);
            */
            timeElapsed++;
            powerBarSlider.value = Mathf.RoundToInt(Power);
            powerText.text = powerBarSlider.value.ToString();
        }

        private IEnumerator AnimateCrosshairAccordingToPower()
        {
            if (Power >= 99.5f)
            {
                crosshairAnimator.SetBool("Perfect",true);
                yield return new WaitForSeconds(2.0f);
            }
            else
            {
                crosshairAnimator.SetBool("FadeOut",true);
                yield return new WaitForSeconds(1.0f);
            }
            this.gameObject.SetActive(false);
        }
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            switch (crosshairState)
            {
                case CrosshairState.OnMoveHorizontally:
                    MoveCrosshairHorizontally(deflection.x);
                    if (InputManager.IsScreenTapted())
                    {
                        timeElapsed = 0;
                        crosshairState = CrosshairState.OnMoveVertically;
                    }
                    break;

                case CrosshairState.OnMoveVertically:
                    MoveCrosshairVertically(deflection.y);
                    if (InputManager.IsScreenTapted())
                    {
                        timeElapsed = 0;
                        crosshairState = CrosshairState.OnPowerSelection;
                        powerBarSlider.gameObject.SetActive(true);
                        powerImage.gameObject.SetActive(true);
                    }
                    break;

                case CrosshairState.OnPowerSelection:
                    PowerPingPong();
                    if (InputManager.IsScreenTapted())
                    {
                        crosshairState = CrosshairState.None;
                        StartCoroutine(AnimateCrosshairAccordingToPower());
                    }
                    break;

                case CrosshairState.None:
                    break;
                    
                default:
                    break;
            }
        }

        private void OnEnable()
        {
            crosshairState = CrosshairState.OnMoveHorizontally;
            crosshairAnimator.SetBool("Perfect",false);
            crosshairAnimator.SetBool("FadeOut",false);
            powerBarSlider.gameObject.SetActive(false);
            powerImage.gameObject.SetActive(false);
            horizontalAxisImage.gameObject.SetActive(true);
            verticalAxisImage.gameObject.SetActive(true);
            newPosition = Vector3.zero;
            timeElapsed = 0;
        }
    }
}