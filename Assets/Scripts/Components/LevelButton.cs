using Systems;
using TMPro;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Components
{
    public class LevelButton : MonoBehaviour, IInteractable
    {
        [SerializeField] private TextMeshProUGUI text;
        public int index;
        public Status status;
        public void Interact()
        {
            if (status == Status.Locked)
                return;
            MainMenu.Instance.onLevelSelected?.Invoke();
            LevelManager.Instance.EnterLevelIndex(index);
        }
        
        public enum Status
        {
            Locked,
            Open,
            Complete
        }
    }
}