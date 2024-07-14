using System.Collections.Generic;
using System.Linq;
using Components;
using Model;
using UnityEngine;
using UnityEngine.Events;
using UnityUtils;
using UtilityToolkit.Editor;
using UtilityToolkit.Runtime;
using Animator = Animation.Animator;

namespace Systems
{
    public class MainMenu : Singleton<MainMenu>
    {
        [SerializeField] private LevelButton levelButtonPrefab;
        private List<LevelButton> sceneLevelButtons;

        private readonly Location[] spawnLocations =
        {
            new(3, 4),
            new(2, 3),
            new(1, 2),
            new(0, 1),
            new(-1, 0),

            new(4, 3),
            new(3, 2),
            new(2, 1),
            new(1, 0),
            new(0, -1),
        };

        public UnityEvent onWorldSelected;
        public UnityEvent onLevelSelected;
        public UnityEvent onMenuSelected;

        public void MenuSelected() => onMenuSelected?.Invoke();

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
            levelButton.SetText((index + 1).ToString());
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

        public void SetTextOnButtonsActive(bool active)
        {
            foreach (var sceneLevelButton in sceneLevelButtons)
            {
                sceneLevelButton.SetTextActive(active);
            }
        }
    }
}