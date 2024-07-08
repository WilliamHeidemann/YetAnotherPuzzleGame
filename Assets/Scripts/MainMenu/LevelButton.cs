using Components;
using Systems;
using UnityEngine;

namespace MainMenu
{
    public class LevelButton : MonoBehaviour, IInteractable
    {
        public int index;
        public void Interact()
        {
            LevelManager.Instance.EnterLevelIndex(index);
        }
    }
}