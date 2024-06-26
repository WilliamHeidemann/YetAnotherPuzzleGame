﻿using System;
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
        public Level current { get; private set; }
        private int currentLevelIndex;
        public bool isLevelComplete { get; private set; }

        private void Start()
        {
            isLevelComplete = false;
            EnterLevel(levels[currentLevelIndex]);
        }

        public void CheckCompletion(IEnumerable<Block> board)
        {
            isLevelComplete = current.targetConfiguration.TrueForAll(board.Contains);
            if (isLevelComplete)
            {
                currentLevelIndex++;
                EnterLevel(levels[currentLevelIndex]);
            }
        }

        public void EnterLevel(Level level)
        {
            current = level;
            isLevelComplete = false;
            Controller.Instance.Initialize(level, this);
        }
    }
}