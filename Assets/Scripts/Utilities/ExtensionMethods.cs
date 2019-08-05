using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WreckingBall
{
    /// <summary>
    /// Extension base class.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// This method finds reference from chields of the transform with the specified name.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="referenceName"></param>
        /// <returns></returns>
        public static GameObject FindReferenceOfChild(this Transform transform,string referenceName)
        {
            foreach (Transform t in transform)
            {
                if (t.gameObject.name.Equals(referenceName))
                {
                    return t.gameObject;
                }
            }
            return null;
        }
        
        /// <summary>
        /// This method sets active or disactive not only the gameobject's itself but also the childs of it.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="state"></param>
        public static void SetActiveWithChildrens(this GameObject gameObject,bool state)
        {
            foreach (Transform transform in gameObject.transform)
            {
                transform.gameObject.SetActive(state);
            }
        }
        
        /// <summary>
        /// This method sets zero to position and the rotation of the transform.
        /// </summary>
        /// <param name="transform"></param>
        public static void SetZeroPositionAndRotation(this Transform transform)
        {
            transform.position=Vector3.zero;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}

