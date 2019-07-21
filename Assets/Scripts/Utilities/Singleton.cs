using UnityEngine;

namespace WreckingBall
{
    /// <summary>
    /// Base class for singleton objects. Singleton classes inherits from this base class.
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private T instance;
        public T Instance
        {
            get => instance;
            set => instance = value;
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            if (instance == null)
            {
                instance = (T) FindObjectOfType(typeof(T));
                if (instance != null)
                {
                    DontDestroyOnLoad(instance);
                }
                else
                {
                    GameObject go = new GameObject();
                    instance = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                }
            }
        }
    }
}