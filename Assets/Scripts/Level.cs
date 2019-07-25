using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Level
{
    public int Id;
    public bool IsUnlocked;
    public string Name;
    public int CollectedStarAmount;
}
