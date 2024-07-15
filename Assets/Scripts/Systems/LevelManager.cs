﻿using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using Model;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Runtime;

namespace Systems
{
    public class LevelManager : Singleton<LevelManager>
    {
        private Level[] levels => GetLevels();
        [SerializeField] private Level[] trialWorld;
        [SerializeField] private Level[] worldOne;
        [SerializeField] private Level[] frogWorld;
        private World currentWorld;
        public World world => currentWorld;
        public Level currentLevel { get; private set; }
        private int levelsCompletedThisWorld => levels.Count(SaveSystem.HasBeenCompleted);
        
        [Space] [SerializeField] private bool showLevelName;
        [SerializeField] private TextMeshProUGUI levelName;

        public void EnterLevelIndex(int index)
        {
            if (index >= levels.Length)
            {
                Debug.LogWarning("Level index out of bounds");
                return;
            }

            EnterLevel(levels[index]);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
                LevelComplete();
        }

        public void LevelComplete()
        {
            SaveSystem.SetComplete(currentLevel);
            
            // 1. Find an open level
            // 2. If all levels have been completed
            // 2.1 If the last level completed was the last level of the world -> go to main menu
            // 2.2 If not, the just increment the level index
            bool OpenLevel(Level l) => !SaveSystem.HasBeenCompleted(l) && levelsCompletedThisWorld >= l.levelsRequiredToUnlock;
            var firstOpen = levels.FirstOption(OpenLevel);
            if (firstOpen.IsSome(out var firstOpenLevel))
            {
                EnterLevel(firstOpenLevel);
                return;
            }

            if (currentLevel == levels[^1])
            {
                MainMenu.Instance.MenuSelected();
                return;
            }

            void EnterNextLevel(int i)
            {
                if (levels[i] == currentLevel)
                    EnterLevelIndex(i + 1);
            }
            
            For.Range(levels.Length, EnterNextLevel);
        }

        private void EnterLevel(Level level)
        {
            currentLevel = level;
            Controller.Instance.Initialize(level);
            levelName.text = level.name;
            levelName.gameObject.SetActive(showLevelName);
        }

        private Level[] GetLevels() => currentWorld switch
        {
            World.Trial => trialWorld,
            World.One => worldOne,
            World.Frog => frogWorld,
            _ => throw new ArgumentOutOfRangeException(nameof(currentWorld), currentWorld, null)
        };

        public void SetWorld(World world) => currentWorld = world;

        public enum World
        {
            Trial,
            One,
            Frog
        }

        public LevelButton.Status GetStatus(int levelButtonIndex)
        {
            var level = levels[levelButtonIndex];
            if (SaveSystem.HasBeenCompleted(level))
                return LevelButton.Status.Complete;

            return levelsCompletedThisWorld >= level.levelsRequiredToUnlock
                ? LevelButton.Status.Open
                : LevelButton.Status.Locked;
        }
    }
}