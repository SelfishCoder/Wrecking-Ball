using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WreckingBall
{
    public class LevelButtonCreator  : MonoBehaviour
    {
        [SerializeField] private Button levelButton;

        private void Awake()
        {
            CreateLevelButtons();
        }

        private void CreateLevelButtons()
        {
            foreach (Level level in LevelManager.Instance.levels)
            {
                Button levelButtonClone = Instantiate(original: levelButton);
                levelButtonClone.GetComponent<RectTransform>().SetParent(p: this.GetComponent<RectTransform>());
                levelButtonClone.GetComponent<RectTransform>().localScale = new Vector3(x: 1,y: 1,z: 1);
                levelButtonClone.GetComponentInChildren<Text>().text = level.Id.ToString();
                levelButtonClone.interactable = level.IsUnlocked;
                levelButtonClone.onClick.AddListener(delegate{LoadLevel(level.Name);});
            }
        }
        
        public void LoadLevel(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            GameManager.CurrentGameState = GameState.InGame;
        }
    }
}