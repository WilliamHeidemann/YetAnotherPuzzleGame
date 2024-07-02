using Systems;
using UnityEngine;
using Animator = Animation.Animator;

namespace Components
{
    public class UndoButton : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Selector.Instance.Undo();
            Animator.Squish(gameObject);
        }
    }
}