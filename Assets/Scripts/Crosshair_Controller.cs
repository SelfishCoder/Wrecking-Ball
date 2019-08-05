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
    [DisallowMultipleComponent, RequireComponent(typeof(Animator))]
    public class Crosshair_Controller : MonoBehaviour
    {
        #region Variables
        [Header("Crosshair Configuration")]
        [SerializeField] private CrosshairState crosshairState;
        [SerializeField] private Vector2 canvasReferenceResolution = new Vector2(1080, 1920);
        [SerializeField] private Vector3 deflection;
        [SerializeField] private float crosshairHorizontalSpeed = 3.25f;
        [SerializeField] private float crosshairVerticalSpeed = 3.5f;
        [SerializeField] private float powerbarFillSpeed = 0.03f;
        [SerializeField] private Vector2 crosshairSize;

        [Header("References to UI Elements")] 
        [SerializeField] private Image horizontalAxisImage;
        [SerializeField] private Image verticalAxisImage;
        [SerializeField] private Image powerImage;
        [SerializeField] private Slider powerBarSlider;
        [SerializeField] private Text powerText;
        [SerializeField] private Text perfectText;
        [SerializeField] private Text missedText;
        [SerializeField] private Animator crosshairAnimator;

        [Header("References to GameObjects")]
        [SerializeField] private WreckingBall_Controller wreckingBall;
        
        /// <summary>
        /// Temporary variables.
        /// </summary>
        [Header("Temporary Variables")]
        [SerializeField] private Vector3 newPosition = Vector3.zero;
        [SerializeField] private float timeElapsed;
        private RaycastHit raycastHit;
        private Ray raycast;
        
        private static float power;
        public static float Power
        {
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

        #endregion

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            ResetValues();
            crosshairAnimator = this.GetComponent<Animator>();
            crosshairSize = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x, this.GetComponent<RectTransform>().sizeDelta.y);
            CalculateAndSetDeflectionOfCrosshair();
        }
        
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (GameManager.CurrentGameState == GameState.InGame)
            {
                switch (crosshairState)
                {
                    case CrosshairState.OnMoveHorizontally:
                        MoveCrosshairHorizontally();
                        if (InputManager.IsScreenTapted())
                        {
                            ChangeCrosshairState(CrosshairState.OnMoveVertically);
                        }
                        break;

                    case CrosshairState.OnMoveVertically:
                        MoveCrosshairVertically();
                        if (InputManager.IsScreenTapted())
                        {
                            if (CastRayToScreenPoint(transform.position))
                            {
                                if (raycastHit.collider.name.Equals("PerfectPoint"))
                                {
                                   StartCoroutine( OnPerfect(raycastHit.collider.transform.position));
                                }
                                else
                                {
                                    if (raycastHit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Obstacles")))
                                    {
                                        powerImage.gameObject.SetActive(true);
                                        powerText.gameObject.SetActive(true);
                                        powerBarSlider.gameObject.SetActive(true);
                                        ChangeCrosshairState(CrosshairState.OnPowerSelection);
                                    }
                                }
                            }
                            else
                            {
                               StartCoroutine( OnMissed());
                            }
                            
                        }
                        break;

                    case CrosshairState.OnPowerSelection:
                        PowerbarPingPong();
                        if (InputManager.IsScreenTapted())
                        {
                            if (Power.Equals(100.0f))
                            {
                                StartCoroutine(OnHundred(raycastHit.collider.transform.position));
                            }
                            else
                            {
                                StartCoroutine(OnHit(raycastHit.collider.transform.position));
                            }
                        }
                        break;

                    case CrosshairState.None:
                        break;
                }    
            }
        }
        
        /// <summary>
        /// Resets the Crosshair values, sets the state to MoveHorizontally and Starts the Animate Coroutine. (FadeIn)
        /// </summary>
        /// <returns>StartCoroutine</returns>
        private IEnumerator ResetCrosshair()
        {
            ResetValues();
            yield return StartCoroutine(Animate("FadeIn",1f));
            ChangeCrosshairState(CrosshairState.OnMoveHorizontally);
        }

        /// <summary>
        /// Changes the state of Crosshair to none.
        /// Decrease lifeCounter -1.
        /// Starts the Animate Coroutine. (Missed)
        /// When the coroutine finishes, Resets the Crosshair.
        /// </summary>
        /// <returns></returns>
        private IEnumerator OnMissed()
        {
            ChangeCrosshairState(CrosshairState.None);
            PlayerStats.Lives--;
            yield return StartCoroutine(Animate("Missed",1f));
            StartCoroutine(ResetCrosshair());
        }

        /// <summary>
        /// Changes the state of Crosshair to none.
        /// Decrease lifeCounter -1.
        /// Starts the Animate Coroutine. (Perfect)
        /// </summary>
        /// <param name="target"></param>
        /// <returns>StartCoroutine</returns>
        private IEnumerator OnPerfect(Vector3 target)
        {
            crosshairState = CrosshairState.None;
            PlayerStats.Lives--;
            yield return StartCoroutine(Animate("Perfect", 2f));
        }

        /// <summary>
        /// This method is called when the power is different from hundred.
        /// Releases the Wrecking ball and destruct the obstacle according to power. 
        /// </summary>
        /// <param name="target">Position of the target point.</param>
        /// <returns>StartCoroutine</returns>
        private IEnumerator OnHit(Vector3 target)
        {
            crosshairState = CrosshairState.None;
            PlayerStats.Lives--;
            yield return StartCoroutine(Animate("FadeOut",1f));
            wreckingBall.gameObject.SetActive(true);
            wreckingBall.Release(target);
            UiManager.Instance.CloseCrosshair();
        }

        /// <summary>
        /// This method is called when the power is hundred.
        /// Releases the wrecking ball and destruct all of the obstacle.
        /// </summary>
        /// <param name="target">Position of the target point</param>
        /// <returns>StartCoroutine</returns>
        private IEnumerator OnHundred(Vector3 target)
        {
            ChangeCrosshairState(CrosshairState.None);
            PlayerStats.Lives--;
            perfectText.gameObject.SetActive(true);
            powerText.gameObject.SetActive(true);
            powerImage.gameObject.SetActive(true);
            yield return StartCoroutine(Animate("Perfect",2f));
            wreckingBall.gameObject.SetActive(true);
            wreckingBall.Release(target);
            UiManager.Instance.CloseCrosshair();
        }
        
        /// <summary>
        /// Cast a ray to the specified point.
        /// </summary>
        /// <param name="ScreenPoint">Position where the ray should cast.</param>
        /// <returns>True or false</returns>
        private bool CastRayToScreenPoint(Vector3 ScreenPoint)
        {
            raycast = Camera.main.ScreenPointToRay((ScreenPoint));
            return Physics.Raycast(raycast, out raycastHit);
        }
        
        /// <summary>
        /// This method changes the Crosshair's state according to specified state.
        /// </summary>
        /// <param name="state">Target state</param>
        private void ChangeCrosshairState(CrosshairState state)
        {
            timeElapsed = 0;
            crosshairState = state;
        }
        
        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            crosshairState = CrosshairState.None;
            StartCoroutine(ResetCrosshair());
        }

        /// <summary>
        /// Resets the Crosshair values.
        /// </summary>
        private void ResetValues()
        {
            //Reset Animations
            crosshairAnimator.SetBool("Perfect", false);
            crosshairAnimator.SetBool("FadeOut", false);
            crosshairAnimator.SetBool("FadeIn", false);
            crosshairAnimator.SetBool("Missed", false);
            //Reset UI
            powerBarSlider.gameObject.SetActive(false);
            powerText.gameObject.SetActive(false);
            powerImage.gameObject.SetActive(false);
            perfectText.gameObject.SetActive(false);
            missedText.gameObject.SetActive(false);
            horizontalAxisImage.gameObject.SetActive(true);
            verticalAxisImage.gameObject.SetActive(true);
            //Reset temporary variables
            transform.localPosition = Vector3.zero;
            newPosition=Vector3.zero;
            timeElapsed = 0;
        }

        /// <summary>
        /// Animate Crosshair with the given Animation parameter by setting it true or false.
        /// Waits for a specified time.
        /// </summary>
        /// <param name="AnimationName">Name of the Animation</param>
        /// <param name="waitTime">Duration of the Animation</param>
        /// <returns></returns>
        private IEnumerator Animate(string AnimationName,float waitTime)
        {
            crosshairAnimator.SetBool(AnimationName,true);
            yield return new WaitForSeconds(waitTime);
            crosshairAnimator.SetBool(AnimationName,false);
        }

        #region Croshair_Movement
        
        /// <summary>
        /// This method calculates how much the crosshair should deflect on each axis.
        /// </summary>
        private void CalculateAndSetDeflectionOfCrosshair()
        {
            float deflectionX = (((canvasReferenceResolution.x / 2) / 10) * 9.75f) - (crosshairSize.x / 2);
            float deflectionY = (((canvasReferenceResolution.y / 2) / 10) * 8.0f) - (crosshairSize.y / 2);
            deflection = new Vector3(deflectionX, deflectionY, 0);
        }
        
        /// <summary>
        /// This methods moves the crosshair at the x axis with the specified deflection.
        /// </summary>
        public void MoveCrosshairHorizontally()
        {
            newPosition.x = Mathf.Sin(timeElapsed * crosshairHorizontalSpeed) * deflection.x;
            transform.localPosition = newPosition;
            timeElapsed += Time.deltaTime;
        }
        
        /// <summary>
        /// This methods moves the crosshair at the y axis with the specified deflection.
        /// </summary>
        public void MoveCrosshairVertically()
        {
            newPosition.y = Mathf.Sin(timeElapsed * crosshairVerticalSpeed) * deflection.y;
            transform.localPosition = newPosition;
            timeElapsed += Time.deltaTime;
        }
        /// <summary>
        /// This method moves powerbar upside down between 0-100 until the screen is tap.
        /// </summary>
        public void PowerbarPingPong()
        {
            //This statement changes the value of power between 0 and 100 gradually.
            power = Mathf.RoundToInt(Mathf.Abs(Mathf.Sin(timeElapsed * powerbarFillSpeed) * 100));
            /*
                //This statement changes the value of power between 0 and 100 linearly. (Alternative)
                power = Mathf.PingPong(timeElapsed * powerbarFillSpeed, 100f);
            */
            timeElapsed++;
            powerBarSlider.value = power;
            powerText.text = powerBarSlider.value.ToString();
        }
        
        #endregion
    }
}