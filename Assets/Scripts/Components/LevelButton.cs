﻿using Systems;
using TMPro;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Components
{
    public class LevelButton : MonoBehaviour, IInteractable
    {
        [SerializeField] private TextMeshProUGUI text;
        public int index;
        public void Interact()
        {
            MainMenu.Instance.onLevelSelected?.Invoke();
            LevelManager.Instance.EnterLevelIndex(index);
        }

        public void SetText(string newText)
        {
            text.text = newText;
        }

        public void SetTextActive(bool active)
        {
            var color = text.color.SetAlpha(active ? 1f : 0f);
            text.CrossFadeColor(color, 1f, false, true);
        }
    }
}