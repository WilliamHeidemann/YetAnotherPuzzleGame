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
        public bool isLevelComplete { get; private set; }
        [Space] [SerializeField] private bool showLevelName;
        [SerializeField] private TextMeshProUGUI levelName;

        public void CheckCompletion(IEnumerable<Block> board)
        {
            isLevelComplete = current.targetConfiguration.TrueForAll(board.Contains);
            if (isLevelComplete)
            {
                currentLevelIndex++;
                EnterLevel(levels[currentLevelIndex]);
            }
        }

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
            print("enter level");
            EnterLevel(levels[index]);
        }

        private void EnterLevel(Level level)
        {
            current = level;
            isLevelComplete = false;
            Controller.Instance.Initialize(level, this);
            levelName.text = level.name;
            levelName.gameObject.SetActive(showLevelName);
        }
    }
}