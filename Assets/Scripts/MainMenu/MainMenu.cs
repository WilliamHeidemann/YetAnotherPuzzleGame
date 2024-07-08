using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Editor;
using UtilityToolkit.Runtime;
using Animator = Animation.Animator;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private LevelButton levelButtonPrefab;
        private List<LevelButton> sceneLevelButtons;

        private readonly Location[] spawnLocations =
        {
            new(-1, 4),
            new(0, 3),
            new(1, 2),
            new(2, 1),
            new(3, 0),

            new(-2, 3),
            new(-1, 2),
            new(0, 1),
            new(1, 0),
            new(2, -1),
        };

        private void Start()
        {
            DisplayLevelButtons();
        }

        public void DisplayLevelButtons()
        {
            SpawnLevelButtons();
            Animator.LevelButtonsIn(sceneLevelButtons.Select(b => b.gameObject));
        }
        
        [Button]
        public void SpawnLevelButtons()
        {
            sceneLevelButtons = new List<LevelButton>();
            For.Range(spawnLocations.Length, SpawnLevelButton);
        }

        private void SpawnLevelButton(int index)
        {
            var levelButton = Instantiate(levelButtonPrefab);
            levelButton.transform.position = spawnLocations[index].asVector3.With(y: -1);
            sceneLevelButtons.Add(levelButton);
            levelButton.index = index;
        }

        [Button]
        public void DeleteLevelButtons()
        {
            foreach (var sceneLevelButton in sceneLevelButtons)
            {
                DestroyImmediate(sceneLevelButton.gameObject);
            }
            sceneLevelButtons.Clear();
        }
    }
}