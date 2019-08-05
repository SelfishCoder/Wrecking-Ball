using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base data class for the Level objects.
/// </summary>
[System.Serializable]
public class Level
{
    public int Id;
    public bool IsUnlocked;
    public string Name;
    public int CollectedStarAmount;
    public Button levelButton;
    public GameObject Obstacle;
}
