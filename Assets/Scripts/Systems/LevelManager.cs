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
        private Level[] levels => GetLevels();
        [SerializeField] private Level[] worldOne;
        [SerializeField] private Level[] frogWorld;
        private World currentWorld;
        public Level currentLevel { get; private set; }
        private int currentLevelIndex;
        [Space] [SerializeField] private bool showLevelName;
        [SerializeField] private TextMeshProUGUI levelName;

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
                EnterNextLevel();
        }

        public void EnterNextLevel()
        {
            currentLevelIndex++;
            EnterLevel(levels[currentLevelIndex]);
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
            World.One => worldOne,
            World.Frog => frogWorld,
            _ => throw new ArgumentOutOfRangeException(nameof(currentWorld), currentWorld, null)
        };

        public void SetWorld(World world) => currentWorld = world;

        public enum World
        {
            One,
            Frog
        }
    }
}