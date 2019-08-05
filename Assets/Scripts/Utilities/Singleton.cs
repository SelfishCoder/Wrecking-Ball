using UnityEngine;

namespace WreckingBall
{
    /// <summary>
    /// Base class for singleton objects. Singleton classes inherits from this base class.
    /// </summary>
    public class Singleton<TypeOfSingleton> : MonoBehaviour where TypeOfSingleton: MonoBehaviour
    {
        /// <summary>
        /// Property of the instance object of the class which is derived from this base class.
        /// </summary>
        public static TypeOfSingleton Instance { get; private set; }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = (TypeOfSingleton) FindObjectOfType(typeof(TypeOfSingleton));
                if (Instance != null)
                {
                    DontDestroyOnLoad(Instance);
                }
                else
                {
                    GameObject go = new GameObject();
                    Instance = go.AddComponent<TypeOfSingleton>();
                    DontDestroyOnLoad(go);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}