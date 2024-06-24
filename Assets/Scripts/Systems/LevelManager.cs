using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using ScriptableObjects;
using UnityEngine;

namespace Systems
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Level[] levels;
        private Level current;
        private int currentLevelIndex;
        public bool isLevelComplete { get; private set; }

        private void Start()
        {
            EnterLevel(levels[currentLevelIndex]);
        }

        public void CheckCompletion(IEnumerable<Block> board)
        {
            isLevelComplete = board.SequenceEqual(current.targetConfiguration);
            if (isLevelComplete)
            {
                currentLevelIndex++;
                EnterLevel(levels[currentLevelIndex]);
            }
        }

        public void EnterLevel(Level level)
        {
            current = level;
            Controller.Instance.Initialize(level, this);
        }
    }
}