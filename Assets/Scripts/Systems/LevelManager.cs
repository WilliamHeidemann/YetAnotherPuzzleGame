using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityUtils;

namespace Systems
{
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private Level[] levels;
        public Level current { get; private set; }
        private int currentLevelIndex;
        [Space] [SerializeField] private bool showLevelName;
        [SerializeField] private TextMeshProUGUI levelName;

        public void SetWorld()
        {
            throw new NotImplementedException();
        }

        public void EnterLevelIndex(int index)
        {
            if (index >= levels.Length)
            {
                Debug.LogWarning("Level index out of bounds");
                return;
            }

            currentLevelIndex = index;
            EnterLevel(levels[index]);
        }

        public void EnterNextLevel()
        {
            currentLevelIndex++;
            EnterLevel(levels[currentLevelIndex]);
        }
        
        private void EnterLevel(Level level)
        {
            current = level;
            Controller.Instance.Initialize(level);
            levelName.text = level.name;
            levelName.gameObject.SetActive(showLevelName);
        }
    }
}