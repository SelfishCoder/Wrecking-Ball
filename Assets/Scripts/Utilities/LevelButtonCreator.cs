using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WreckingBall
{
    public class LevelButtonCreator  : MonoBehaviour
    {
        [SerializeField] private Button levelButtonPrefab;
        
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            CreateLevelButtons();
        }

        /// <summary>
        /// This method creates Level buttons and adds initial values.
        /// </summary>
        private void CreateLevelButtons()
        {
            foreach (Level level in LevelManager.Instance.levels)
            {
                level.levelButton= Instantiate(original: levelButtonPrefab);
                level.levelButton.GetComponent<RectTransform>().SetParent(p: this.GetComponent<RectTransform>());
                level.levelButton.GetComponent<RectTransform>().localScale = new Vector3(x: 1,y: 1,z: 1);
                level.levelButton.GetComponentInChildren<Text>().text = level.Id.ToString();
                level.levelButton.interactable = level.IsUnlocked;
                level.levelButton.onClick.AddListener(delegate { StartCoroutine(LevelManager.Instance.LoadLevel(level)); });
                level.levelButton.name = "Level" + level.Id + "_Button";
            }
        }
    }
}