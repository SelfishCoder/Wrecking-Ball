using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WreckingBall
{
    public static class ExtensionMethods
    {
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
    }
}

