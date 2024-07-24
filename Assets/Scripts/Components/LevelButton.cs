using System;
using Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UtilityToolkit.Runtime;

namespace Components
{
    public class LevelButton : MonoBehaviour, IInteractable
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image image;
        [SerializeField] private Sprite locked;
        [SerializeField] private Sprite open;
        [SerializeField] private Sprite complete;
        public int index;
        public Status status;
        public void Interact()
        {
            if (status == Status.Locked)
                return;
            SoundEffectSystem.Instance.PlayBubble();
            MainMenu.Instance.onLevelSelected?.Invoke();
            LevelManager.Instance.EnterLevelIndex(index);
        }
        
        public enum Status
        {
            Locked,
            Open,
            Complete
        }

        public void UpdateImage()
        {
            image.gameObject.SetActive(true);
            image.sprite = status switch
            {
                Status.Locked => locked,
                Status.Open => open,
                Status.Complete => complete,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void HideImage()
        {
            image.gameObject.SetActive(false);
        }
    }
}