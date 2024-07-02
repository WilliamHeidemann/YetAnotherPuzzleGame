using Systems;
using UnityEngine;

namespace Components
{
    public class UndoButton : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Selector.Instance.Undo();
        }
    }
}