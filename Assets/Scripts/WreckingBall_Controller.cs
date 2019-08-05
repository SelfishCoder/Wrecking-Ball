using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace WreckingBall
{
    /// <summary>
    /// This class controls behaviour of the Wrecking Ball.
    /// </summary>
    public class WreckingBall_Controller : MonoBehaviour
    {
        #region Variables

        [Header("References")]
        [SerializeField] private GameObject Ball;
        [SerializeField] private GameObject Chain;
        [SerializeField] private Rigidbody wreckingBallRigidbody;
        [SerializeField] private HingeJoint wreckingBallHingeJoint;
        
        /// <summary>
        /// Calculation Variables
        /// </summary>
        private float ChainSize;
        private float BallSize;
        private float WreckingBallSize;

        /// <summary>
        /// Temporary Variables
        /// </summary>
        private Rigidbody _rigidbody;
        private bool started;

        #endregion

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            started = false;
        }

        /// <summary>
        /// This method adjusts the position and rotation of Wrecking Ball according to the target point.
        /// </summary>
        /// <param name="target">Position of the target point.</param>
        public void TakePosition(Vector3 target)
        {
            //Reset the last values.
            this.transform.position = Vector3.zero;
            this.transform.rotation = Quaternion.Euler(Vector3.zero);
            wreckingBallRigidbody.velocity = Vector3.zero;
            wreckingBallRigidbody.angularVelocity = Vector3.zero;
            
            //Adjust the position and rotation
            this.transform.rotation = Quaternion.Euler(60, 0, 0);
            ChainSize = Chain.GetComponent<Transform>().localScale.y * 2;
            BallSize = Ball.GetComponent<SphereCollider>().radius;
            WreckingBallSize = BallSize + ChainSize;
            Vector3 newPosition = new Vector3(target.x, target.y + WreckingBallSize, target.z);
            this.transform.position = newPosition;
            wreckingBallHingeJoint.connectedAnchor = newPosition;
        }

        /// <summary>
        /// This Method releases the Wrecking Ball.
        /// </summary>
        /// <param name="target">Position of the target point.</param>
        public void Release(Vector3 target)
        {
            started = false;
            TakePosition(target);
            wreckingBallRigidbody.isKinematic = false;
        }

        
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Obstacles")))
            {
                if (other.gameObject.GetComponent<Obstacle_Cubes>().HitCount == 0)
                {
                    if (!started)
                    {
                        if (Crosshair_Controller.Power.Equals(100))
                        {
                            StartCoroutine(DestructAll());
                        }
                        else
                        {
                           StartCoroutine(OnHit());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This methods is called when the Wrecking Ball hits the target point and creates an area according to power which is given to the Wrecking Ball.
        /// Gets GameObject's in that area and sets their isKinematic property as false.
        /// Updates the necessary UI elements.
        /// </summary>
        /// <returns>new WaitForSeconds</returns>
        IEnumerator OnHit()
        {
            started = true;
            wreckingBallRigidbody.drag = 10f;
            Collider[] colliders = Physics.OverlapCapsule(Ball.transform.position,
                new Vector3(Ball.transform.position.x, transform.position.y, Ball.transform.position.z),
                Crosshair_Controller.Power / 15f);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.layer.Equals(LayerMask.NameToLayer("Obstacles")) &&
                    collider.gameObject.GetComponent<Obstacle_Cubes>().HitCount == 0)
                {
                    _rigidbody = collider.GetComponent<Rigidbody>();
                    _rigidbody.isKinematic = false;
                }
            }
            yield return new WaitForSeconds(5f);
            UiManager.Instance.UpdateLevelProgress(LevelManager.Instance.CalculateLevelProgress());
            UiManager.Instance.OpenCrosshair();
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// Gets all the GameObjects tagged as Obstacle.
        /// Gets GameObject's in that area and sets their isKinematic property as false.
        /// Updates the necessary UI elements.
        /// </summary>
        /// <returns>new WaitForSeconds</returns>
        IEnumerator DestructAll()
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Obstacle");
            foreach (GameObject gameobject in gameObjects)
            {
                if (gameobject.layer.Equals(LayerMask.NameToLayer("Obstacles")))
                {
                    _rigidbody = gameobject.GetComponent<Rigidbody>();
                    _rigidbody.isKinematic = false;
                }
            }

            yield return new WaitForSeconds(5f);
            UiManager.Instance.UpdateLevelProgress(LevelManager.Instance.CalculateLevelProgress());
            GameManager.CurrentGameState = GameState.Won;
            this.gameObject.SetActive(false);
        }
    }
}