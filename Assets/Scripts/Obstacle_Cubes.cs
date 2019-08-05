using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Cubes is used for creating obstacles.
/// </summary>
public class Obstacle_Cubes : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private int hitCount;

    public int HitCount
    {
        get { return hitCount; }
        set
        {
            if (value >= 0)
            {
                hitCount = value;
            }
        }
    }

    private void OnEnable()
    {
        hitCount = 0;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (hitCount>0)
        {
            Physics.IgnoreLayerCollision(7,10,true);
        }
    }

}
